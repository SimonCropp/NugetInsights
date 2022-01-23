// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;

namespace NuGet.Insights
{
    public static class HttpSourceExtensions
    {
        private const int DefaultMaxTries = 3;

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new AssumeUniversalDateTimeConverter(),
                new AssumeUniversalDateTimeOffsetConverter(),
            },
        };

        public static async Task<bool> UrlExistsAsync(
            this HttpSource httpSource,
            string url,
            ILogger logger,
            CancellationToken token = default)
        {
            var nuGetLogger = logger.ToNuGetLogger();
            return await httpSource.ProcessResponseAsync(
                new HttpSourceRequest(() => HttpRequestMessageFactory.Create(HttpMethod.Head, url, nuGetLogger))
                {
                    IgnoreNotFounds = true,
                },
                response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return Task.FromResult(true);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return Task.FromResult(false);
                    }

                    throw new HttpRequestException(
                        $"The request to {url} return HTTP {(int)response.StatusCode} {response.ReasonPhrase}.");
                },
                nuGetLogger,
                token);
        }

        public static Task<T> DeserializeUrlAsync<T>(
            this HttpSource httpSource,
            string url,
            bool ignoreNotFounds,
            ILogger logger,
            CancellationToken token = default)
        {
            return httpSource.DeserializeUrlAsync<T>(
                url,
                ignoreNotFounds,
                maxTries: DefaultMaxTries,
                logger: logger,
                token: token);
        }


        public static Task<T> DeserializeUrlAsync<T>(
            this HttpSource httpSource,
            string url,
            bool ignoreNotFounds,
            int maxTries,
            ILogger logger,
            CancellationToken token = default)
        {
            return httpSource.DeserializeUrlAsync<T>(
                url,
                ignoreNotFounds,
                maxTries: maxTries,
                options: JsonSerializerOptions,
                logger: logger,
                token: token);
        }


        public static async Task<T> DeserializeUrlAsync<T>(
            this HttpSource httpSource,
            string url,
            bool ignoreNotFounds,
            int maxTries,
            JsonSerializerOptions options,
            ILogger logger,
            CancellationToken token = default)
        {
            var nuGetLogger = logger.ToNuGetLogger();
            return await httpSource.ProcessStreamAsync(
                new HttpSourceRequest(url, nuGetLogger)
                {
                    IgnoreNotFounds = ignoreNotFounds,
                    MaxTries = maxTries,
                    RequestTimeout = TimeSpan.FromSeconds(30),
                },
                async stream =>
                {
                    if (stream == null)
                    {
                        return default;
                    }

                    try
                    {
                        return await JsonSerializer.DeserializeAsync<T>(stream, options, token);
                    }
                    catch (JsonException ex)
                    {
                        logger.LogWarning(ex, "Unable to deserialize {Url} as type {TypeName}.", url, typeof(T).FullName);
                        throw;
                    }
                },
                nuGetLogger,
                token);
        }

        public static async Task<BlobMetadata> GetBlobMetadataAsync(
            this HttpSource httpSource,
            string url,
            ILogger logger,
            CancellationToken token = default)
        {
            var nuGetLogger = logger.ToNuGetLogger();

            // Try to get all of the information using a HEAD request.
            var blobMetadata = await httpSource.ProcessResponseAsync(
                new HttpSourceRequest(() => HttpRequestMessageFactory.Create(HttpMethod.Head, url, nuGetLogger)),
                response =>
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return Task.FromResult(new BlobMetadata(
                            exists: false,
                            hasContentMD5Header: false,
                            contentMD5: null));
                    }

                    response.EnsureSuccessStatusCode();

                    var headerMD5Bytes = response.Content.Headers.ContentMD5;
                    if (headerMD5Bytes != null)
                    {
                        var contentMD5 = headerMD5Bytes.ToLowerHex();
                        return Task.FromResult(new BlobMetadata(
                            exists: true,
                            hasContentMD5Header: true,
                            contentMD5: contentMD5));
                    }

                    return Task.FromResult<BlobMetadata>(null);
                },
                nuGetLogger,
                token);

            if (blobMetadata != null)
            {
                return blobMetadata;
            }

            // If no Content-MD5 header was found in the response, calculate the package hash by downloading the
            // package.
            return await httpSource.ProcessStreamAsync(
                new HttpSourceRequest(url, nuGetLogger),
                async stream =>
                {
                    var buffer = new byte[16 * 1024];
                    using (var md5 = MD5.Create())
                    {
                        int read;
                        do
                        {
                            read = await stream.ReadAsync(buffer, 0, buffer.Length);
                            md5.TransformBlock(buffer, 0, read, buffer, 0);
                        }
                        while (read > 0);

                        md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                        var contentMD5 = md5.Hash.ToLowerHex();

                        return new BlobMetadata(
                            exists: true,
                            hasContentMD5Header: false,
                            contentMD5: contentMD5);
                    }
                },
                nuGetLogger,
                token);
        }
    }
}
