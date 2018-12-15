﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Knapcode.ExplorePackages.Logic
{
    public class FileStorageServiceTest : IAsyncLifetime
    {
        private const string Id = "Newtonsoft.Json";
        private const string Version = "9.0.1";

        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;
        private readonly ExplorePackagesSettings _settings;
        private readonly PackageBlobNameProvider _blobNameProvider;
        private readonly Mock<BlobStorageService> _blobStorageService;
        private readonly MemoryCache _memoryCache;
        private readonly FileStorageService _target;

        public FileStorageServiceTest(ITestOutputHelper output)
        {
            _httpClient = new HttpClient();

            _output = output;
            _settings = new ExplorePackagesSettings
            {
                StorageConnectionString = "UseDevelopmentStorage=true",
                StorageContainerName = Guid.NewGuid().ToString("N"),
            };
            _blobNameProvider = new PackageBlobNameProvider();
            _blobStorageService = new Mock<BlobStorageService>(
                _httpClient,
                _settings,
                _output.GetLogger<BlobStorageService>())
            {
                CallBase = true,
            };
            _memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

            _target = new FileStorageService(
                _blobNameProvider,
                _blobStorageService.Object,
                _memoryCache,
                _output.GetLogger<FileStorageService>());
        }

        [Theory]
        [MemberData(nameof(Combinations))]
        public async Task ReturnsNullWithNonExistentFile(bool isStorageContainerPublic, FileArtifactType type)
        {
            // Arrange
            _settings.IsStorageContainerPublic = isStorageContainerPublic;

            // Act
            using (var actual = await _target.GetStreamOrNullAsync(Id, Version, type))
            {
                // Assert
                Assert.Null(actual);

                _blobStorageService.Verify(
                    x => x.UploadStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()),
                    Times.Never);
                _blobStorageService.Verify(
                    x => x.TryDownloadStreamAsync(It.IsAny<string>(), It.IsAny<Stream>()),
                    Times.Once);
            }
        }

        [Theory]
        [MemberData(nameof(Combinations))]
        public async Task CanReplaceExistingFile(bool isStorageContainerPublic, FileArtifactType type)
        {
            // Arrange
            _settings.IsStorageContainerPublic = isStorageContainerPublic;
            var initial = new MemoryStream(Encoding.UTF8.GetBytes("Initial!"));
            var expected = new MemoryStream(Encoding.UTF8.GetBytes("Hello, world!"));

            await _target.StoreStreamAsync(
                Id,
                Version,
                type,
                dest => initial.CopyToAsync(dest));

            _blobStorageService.ResetCalls();

            // Act
            await _target.StoreStreamAsync(
                Id,
                Version,
                type,
                dest => expected.CopyToAsync(dest));

            // Assert
            _blobStorageService.Verify(
                x => x.UploadStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()),
                Times.Once);
            _blobStorageService.Verify(
                x => x.TryDownloadStreamAsync(It.IsAny<string>(), It.IsAny<Stream>()),
                Times.Never);

            _blobStorageService.ResetCalls();

            using (var actual = await _target.GetStreamOrNullAsync(Id, Version, type))
            {
                AssertSameStreams(expected, actual);

                _blobStorageService.Verify(
                    x => x.UploadStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()),
                    Times.Never);
                _blobStorageService.Verify(
                    x => x.TryDownloadStreamAsync(It.IsAny<string>(), It.IsAny<Stream>()),
                    Times.Never);
            }
        }

        [Theory]
        [MemberData(nameof(Combinations))]
        public async Task CanWriteAndReadAFileFromCache(bool isStorageContainerPublic, FileArtifactType type)
        {
            // Arrange
            _settings.IsStorageContainerPublic = isStorageContainerPublic;
            var expected = new MemoryStream(Encoding.UTF8.GetBytes("Hello, world!"));

            // Act & Assert
            await _target.StoreStreamAsync(
                Id,
                Version,
                type,
                dest => expected.CopyToAsync(dest));

            _blobStorageService.Verify(
                x => x.UploadStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()),
                Times.Once);
            _blobStorageService.Verify(
                x => x.TryDownloadStreamAsync(It.IsAny<string>(), It.IsAny<Stream>()),
                Times.Never);

            _blobStorageService.ResetCalls();

            using (var actual = await _target.GetStreamOrNullAsync(Id, Version, type))
            {
                AssertSameStreams(expected, actual);

                _blobStorageService.Verify(
                    x => x.UploadStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()),
                    Times.Never);
                _blobStorageService.Verify(
                    x => x.TryDownloadStreamAsync(It.IsAny<string>(), It.IsAny<Stream>()),
                    Times.Never);
            }
        }

        [Theory]
        [MemberData(nameof(Combinations))]
        public async Task CanWriteAndReadAFileWithoutCache(bool isStorageContainerPublic, FileArtifactType type)
        {
            // Arrange
            _settings.IsStorageContainerPublic = isStorageContainerPublic;
            var expected = new MemoryStream(Encoding.UTF8.GetBytes("Hello, world!"));

            // Act & Assert
            await _target.StoreStreamAsync(
                Id,
                Version,
                type,
                dest => expected.CopyToAsync(dest));

            _blobStorageService.Verify(
                x => x.UploadStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()),
                Times.Once);
            _blobStorageService.Verify(
                x => x.TryDownloadStreamAsync(It.IsAny<string>(), It.IsAny<Stream>()),
                Times.Never);

            _blobStorageService.ResetCalls();
            _memoryCache.Remove($"{Id}/{Version}/{type}".ToLowerInvariant());

            using (var actual = await _target.GetStreamOrNullAsync(Id, Version, type))
            {
                AssertSameStreams(expected, actual);

                _blobStorageService.Verify(
                    x => x.UploadStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()),
                    Times.Never);
                _blobStorageService.Verify(
                    x => x.TryDownloadStreamAsync(It.IsAny<string>(), It.IsAny<Stream>()),
                    Times.Once);
            }
        }

        public static IEnumerable<object[]> Combinations
        {
            get
            {
                foreach (var isStorageContainerPublic in new[] { false, true })
                {
                    foreach (var type in Enum.GetValues(typeof(FileArtifactType)).Cast<FileArtifactType>())
                    {
                        yield return new object[] { isStorageContainerPublic, type };
                    }
                }
            }
        }

        public async Task DisposeAsync()
        {
            await GetContainer().DeleteIfExistsAsync();
        }

        public async Task InitializeAsync()
        {
            var container = GetContainer();
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob,
            });
        }

        private CloudBlobContainer GetContainer()
        {
            return CloudStorageAccount
                .Parse(_settings.StorageConnectionString)
                .CreateCloudBlobClient()
                .GetContainerReference(_settings.StorageContainerName);
        }

        private void AssertSameStreams(Stream expected, Stream actual)
        {
            var expectedBytes = GetStreamBytes(expected);
            var actualBytes = GetStreamBytes(actual);
            Assert.Equal(expectedBytes, actualBytes);
        }

        private byte[] GetStreamBytes(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.Position = 0;
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}