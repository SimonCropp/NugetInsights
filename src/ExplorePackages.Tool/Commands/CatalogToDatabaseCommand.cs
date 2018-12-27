﻿using System.Threading;
using System.Threading.Tasks;
using Knapcode.ExplorePackages.Logic;
using McMaster.Extensions.CommandLineUtils;
using NuGet.CatalogReader;

namespace Knapcode.ExplorePackages.Tool.Commands
{
    public class CatalogToDatabaseCommand : ICommand
    {
        private readonly CatalogReader _catalogReader;
        private readonly CursorService _cursorService;
        private readonly CatalogToDatabaseProcessor _processor;
        private readonly ISingletonService _singletonService;

        public CatalogToDatabaseCommand(
            CatalogReader catalogReader,
            CursorService cursorService,
            CatalogToDatabaseProcessor processor,
            ISingletonService singletonService)
        {
            _catalogReader = catalogReader;
            _cursorService = cursorService;
            _processor = processor;
            _singletonService = singletonService;
        }

        public void Configure(CommandLineApplication app)
        {
        }

        public async Task ExecuteAsync(CancellationToken token)
        {
            var catalogProcessor = new CatalogProcessorQueue(
                _catalogReader,
                _cursorService,
                _processor,
                _singletonService);
            await catalogProcessor.ProcessAsync(token);
        }

        public bool IsDatabaseRequired() => true;
        public bool IsReadOnly() => false;
    }
}
