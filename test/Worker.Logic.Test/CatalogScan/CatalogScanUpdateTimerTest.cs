﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace Knapcode.ExplorePackages.Worker
{
    public class CatalogScanUpdateTimerTest : BaseWorkerLogicIntegrationTest
    {
        public CatalogScanUpdateTimerTest(ITestOutputHelper output, DefaultWebApplicationFactory<StaticFilesStartup> factory) : base(output, factory)
        {
        }

        protected override void ConfigureHostBuilder(IHostBuilder hostBuilder)
        {
            base.ConfigureHostBuilder(hostBuilder);

            hostBuilder.ConfigureServices(serviceCollection =>
            {
                serviceCollection.AddTransient<CatalogScanUpdateTimer>();
            });
        }

        public CatalogScanUpdateTimer Target => Host.Services.GetRequiredService<CatalogScanUpdateTimer>();

        [Fact]
        public async Task StartsCatalogScansAsync()
        {
            await Target.InitializeAsync();

            var result = await Target.ExecuteAsync();

            var catalogScans = await CatalogScanStorageService.GetIndexScansAsync();
            Assert.NotEmpty(catalogScans);
            Assert.True(result);
        }

        [Fact]
        public async Task DoesNotStartCatalogScansWhenScansAreAlreadyRunningAsync()
        {
            await Target.InitializeAsync();
            await CatalogScanService.UpdateAllAsync(max: null);

            var result = await Target.ExecuteAsync();

            var catalogScans = await CatalogScanStorageService.GetIndexScansAsync();
            Assert.NotEmpty(catalogScans);
            Assert.False(result);
        }
    }
}
