// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace NuGet.Insights
{
    public delegate Task<HttpResponseMessage> GetResponseAsync(CancellationToken token);
    public delegate Task<HttpResponseMessage> SendMessageAsync(HttpRequestMessage request, CancellationToken token);
    public delegate Task<HttpResponseMessage> SendMessageWithBaseAsync(HttpRequestMessage request, SendMessageAsync baseSendAsync, CancellationToken token);

    public class TestHttpMessageHandlerFactory : INuGetInsightsHttpMessageHandlerFactory
    {
        public SendMessageWithBaseAsync OnSendAsync { get; set; }

        private ConcurrentQueue<HttpRequestMessage> Requests { get; } = new ConcurrentQueue<HttpRequestMessage>();

        public ConcurrentQueue<HttpResponseMessage> Responses { get; } = new ConcurrentQueue<HttpResponseMessage>();

        public IEnumerable<HttpRequestMessage> SuccessRequests => Responses
            .Where(x => x.IsSuccessStatusCode && x.RequestMessage is not null)
            .Select(x => x.RequestMessage);

        public void Clear()
        {
            Requests.Clear();
            Responses.Clear();
        }

        public DelegatingHandler Create()
        {
            return new TestHttpMessageHandler(async (req, baseSendAsync, token) =>
            {
                if (OnSendAsync != null)
                {
                    return await OnSendAsync(req, baseSendAsync, token);
                }

                return null;
            }, Requests, Responses);
        }
    }
}
