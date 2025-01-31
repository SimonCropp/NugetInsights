// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Insights.Worker.NuGetPackageExplorerToCsv
{
    public class NuGetPackageExplorerToCsvIntegrationTest : BaseCatalogLeafScanToCsvIntegrationTest<NuGetPackageExplorerRecord, NuGetPackageExplorerFile>
    {
        private const string NuGetPackageExplorerToCsvDir = nameof(NuGetPackageExplorerToCsv);
        private const string NuGetPackageExplorerToCsv_WithDeleteDir = nameof(NuGetPackageExplorerToCsv_WithDelete);

        [Fact]
        public async Task NuGetPackageExplorerToCsv()
        {
            // Arrange
            var min0 = DateTimeOffset.Parse("2020-11-10T23:37:14.085963Z", CultureInfo.InvariantCulture);
            var max1 = DateTimeOffset.Parse("2020-11-10T23:38:46.5558967Z", CultureInfo.InvariantCulture);
            var max2 = DateTimeOffset.Parse("2020-11-10T23:39:05.5717605Z", CultureInfo.InvariantCulture);

            await CatalogScanService.InitializeAsync();
            await SetCursorAsync(CatalogScanDriverType.LoadLatestPackageLeaf, max2);
            await SetCursorAsync(min0);

            // Act
            await UpdateAsync(max1);

            // Assert
            await AssertBlobCountAsync(DestinationContainerName1, 2); // bucket 0 does not exist
            await AssertBlobCountAsync(DestinationContainerName2, 2); // bucket 0 does not exist
            await AssertOutputAsync(NuGetPackageExplorerToCsvDir, Step1, 1);
            await AssertOutputAsync(NuGetPackageExplorerToCsvDir, Step1, 2);

            // Act
            await UpdateAsync(max2);

            // Assert
            await AssertOutputAsync(NuGetPackageExplorerToCsvDir, Step2, 0);
            await AssertOutputAsync(NuGetPackageExplorerToCsvDir, Step1, 1); // This file is unchanged.
            await AssertOutputAsync(NuGetPackageExplorerToCsvDir, Step2, 2);
        }

        [Fact]
        public async Task NuGetPackageExplorerToCsv_WithDelete()
        {
            // Arrange
            MakeDeletedPackageAvailable();
            var min0 = DateTimeOffset.Parse("2020-12-20T02:37:31.5269913Z", CultureInfo.InvariantCulture);
            var max1 = DateTimeOffset.Parse("2020-12-20T03:01:57.2082154Z", CultureInfo.InvariantCulture);
            var max2 = DateTimeOffset.Parse("2020-12-20T03:03:53.7885893Z", CultureInfo.InvariantCulture);

            await CatalogScanService.InitializeAsync();
            await SetCursorAsync(CatalogScanDriverType.LoadLatestPackageLeaf, max2);
            await SetCursorAsync(min0);

            // Act
            await UpdateAsync(max1);

            // Assert
            await AssertOutputAsync(NuGetPackageExplorerToCsv_WithDeleteDir, Step1, 0);
            await AssertOutputAsync(NuGetPackageExplorerToCsv_WithDeleteDir, Step1, 1);
            await AssertOutputAsync(NuGetPackageExplorerToCsv_WithDeleteDir, Step1, 2);

            // Act
            await UpdateAsync(max2);

            // Assert
            await AssertOutputAsync(NuGetPackageExplorerToCsv_WithDeleteDir, Step1, 0); // This file is unchanged.
            await AssertOutputAsync(NuGetPackageExplorerToCsv_WithDeleteDir, Step1, 1); // This file is unchanged.
            await AssertOutputAsync(NuGetPackageExplorerToCsv_WithDeleteDir, Step2, 2);
        }

        public NuGetPackageExplorerToCsvIntegrationTest(ITestOutputHelper output, DefaultWebApplicationFactory<StaticFilesStartup> factory)
            : base(output, factory)
        {
        }

        protected override string DestinationContainerName1 => Options.Value.NuGetPackageExplorerContainerName;
        protected override string DestinationContainerName2 => Options.Value.NuGetPackageExplorerFileContainerName;
        protected override CatalogScanDriverType DriverType => CatalogScanDriverType.NuGetPackageExplorerToCsv;
        public override IEnumerable<CatalogScanDriverType> LatestLeavesTypes => new[] { DriverType };
        public override IEnumerable<CatalogScanDriverType> LatestLeavesPerIdTypes => Enumerable.Empty<CatalogScanDriverType>();

        protected override IEnumerable<string> GetExpectedCursorNames()
        {
            return base.GetExpectedCursorNames().Concat(new[] { "CatalogScan-" + CatalogScanDriverType.LoadLatestPackageLeaf });
        }
    }
}
