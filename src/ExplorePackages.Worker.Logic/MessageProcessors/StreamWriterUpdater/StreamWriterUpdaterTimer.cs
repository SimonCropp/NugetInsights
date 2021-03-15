﻿using System;
using System.Threading.Tasks;
using Knapcode.ExplorePackages.Timers;

namespace Knapcode.ExplorePackages.Worker.StreamWriterUpdater
{
    public class StreamWriterUpdaterTimer<T> : ITimer where T : IAsyncDisposable, IAsOfData
    {
        private readonly IStreamWriterUpdaterService<T> _service;
        private readonly IStreamWriterUpdater<T> _updater;

        public StreamWriterUpdaterTimer(
            IStreamWriterUpdaterService<T> service,
            IStreamWriterUpdater<T> updater)
        {
            _service = service;
            _updater = updater;
        }

        public string Name => _updater.OperationName;
        public TimeSpan Frequency => _updater.LoopFrequency;
        public bool IsEnabled => _service.IsEnabled;

        public async Task ExecuteAsync()
        {
            await _service.StartAsync(loop: false, notBefore: TimeSpan.Zero);
        }

        public async Task InitializeAsync()
        {
            await _service.InitializeAsync();
        }
    }
}