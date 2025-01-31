// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Insights.Worker.PackageCompatibilityToCsv
{
    public class PackageCompatibilityToCsvIntegrationTest : BaseCatalogLeafScanToCsvIntegrationTest<PackageCompatibility>
    {
        private const string PackageCompatibilityToCsvDir = nameof(PackageCompatibilityToCsv);
        private const string PackageCompatibilityToCsv_WithManyAssetsDir = nameof(PackageCompatibilityToCsv_WithManyAssets);
        private const string PackageCompatibilityToCsv_WithUnsupportedDir = nameof(PackageCompatibilityToCsv_WithUnsupported);
        private const string PackageCompatibilityToCsv_WithDeleteDir = nameof(PackageCompatibilityToCsv_WithDelete);

        [Fact]
        public async Task PackageCompatibilityToCsv()
        {
            // Arrange
            var min0 = DateTimeOffset.Parse("2020-11-27T19:34:24.4257168Z", CultureInfo.InvariantCulture);
            var max1 = DateTimeOffset.Parse("2020-11-27T19:35:06.0046046Z", CultureInfo.InvariantCulture);
            var max2 = DateTimeOffset.Parse("2020-11-27T19:36:50.4909042Z", CultureInfo.InvariantCulture);

            await CatalogScanService.InitializeAsync();
            await SetCursorsAsync([CatalogScanDriverType.LoadPackageArchive, CatalogScanDriverType.LoadPackageManifest], max2);
            await SetCursorAsync(min0);

            // Act
            await UpdateAsync(max1);

            // Assert
            await AssertOutputAsync(PackageCompatibilityToCsvDir, Step1, 0);
            await AssertOutputAsync(PackageCompatibilityToCsvDir, Step1, 1);
            await AssertOutputAsync(PackageCompatibilityToCsvDir, Step1, 2);

            // Act
            await UpdateAsync(max2);

            // Assert
            await AssertOutputAsync(PackageCompatibilityToCsvDir, Step2, 0);
            await AssertOutputAsync(PackageCompatibilityToCsvDir, Step1, 1); // This file is unchanged.
            await AssertOutputAsync(PackageCompatibilityToCsvDir, Step2, 2);
        }

        [Fact]
        public async Task PackageCompatibilityToCsv_WithManyAssets()
        {
            // Arrange
            var max1 = DateTimeOffset.Parse("2021-03-22T20:13:54.6075418Z", CultureInfo.InvariantCulture);
            var min0 = max1.AddTicks(-1);

            await CatalogScanService.InitializeAsync();
            await SetCursorAsync(CatalogScanDriverType.LoadPackageArchive, max1);
            await SetCursorAsync(CatalogScanDriverType.LoadPackageManifest, max1);
            await SetCursorAsync(min0);

            // Act
            await UpdateAsync(max1);

            // Assert
            await AssertOutputAsync(PackageCompatibilityToCsv_WithManyAssetsDir, Step1, 0);
            await AssertBlobCountAsync(DestinationContainerName, 1);
        }

        [Fact]
        public async Task PackageCompatibilityToCsv_WithUnsupported()
        {
            // Arrange
            var max1 = DateTimeOffset.Parse("2019-11-18T17:19:38.2541574Z", CultureInfo.InvariantCulture);
            var min0 = max1.AddTicks(-1);

            await CatalogScanService.InitializeAsync();
            await SetCursorAsync(CatalogScanDriverType.LoadPackageArchive, max1);
            await SetCursorAsync(CatalogScanDriverType.LoadPackageManifest, max1);
            await SetCursorAsync(min0);

            // Act
            await UpdateAsync(max1);

            // Assert
            await AssertOutputAsync(PackageCompatibilityToCsv_WithUnsupportedDir, Step1, 0);
            await AssertBlobCountAsync(DestinationContainerName, 1);
        }

        [Fact]
        public async Task PackageCompatibilityToCsv_WithDelete()
        {
            // Arrange
            MakeDeletedPackageAvailable();
            var min0 = DateTimeOffset.Parse("2020-12-20T02:37:31.5269913Z", CultureInfo.InvariantCulture);
            var max1 = DateTimeOffset.Parse("2020-12-20T03:01:57.2082154Z", CultureInfo.InvariantCulture);
            var max2 = DateTimeOffset.Parse("2020-12-20T03:03:53.7885893Z", CultureInfo.InvariantCulture);

            await CatalogScanService.InitializeAsync();
            await SetCursorAsync(CatalogScanDriverType.LoadPackageArchive, max2);
            await SetCursorAsync(CatalogScanDriverType.LoadPackageManifest, max2);
            await SetCursorAsync(min0);

            // Act
            await UpdateAsync(max1);

            // Assert
            await AssertOutputAsync(PackageCompatibilityToCsv_WithDeleteDir, Step1, 0);
            await AssertOutputAsync(PackageCompatibilityToCsv_WithDeleteDir, Step1, 1);
            await AssertOutputAsync(PackageCompatibilityToCsv_WithDeleteDir, Step1, 2);

            // Act
            await UpdateAsync(max2);

            // Assert
            await AssertOutputAsync(PackageCompatibilityToCsv_WithDeleteDir, Step1, 0); // This file is unchanged.
            await AssertOutputAsync(PackageCompatibilityToCsv_WithDeleteDir, Step1, 1); // This file is unchanged.
            await AssertOutputAsync(PackageCompatibilityToCsv_WithDeleteDir, Step2, 2);
        }

        public PackageCompatibilityToCsvIntegrationTest(ITestOutputHelper output, DefaultWebApplicationFactory<StaticFilesStartup> factory)
            : base(output, factory)
        {
        }

        protected override string DestinationContainerName => Options.Value.PackageCompatibilityContainerName;
        protected override CatalogScanDriverType DriverType => CatalogScanDriverType.PackageCompatibilityToCsv;
        public override IEnumerable<CatalogScanDriverType> LatestLeavesTypes => new[] { DriverType };
        public override IEnumerable<CatalogScanDriverType> LatestLeavesPerIdTypes => Enumerable.Empty<CatalogScanDriverType>();

        protected override IEnumerable<string> GetExpectedCursorNames()
        {
            return base.GetExpectedCursorNames().Concat(new[]
            {
                "CatalogScan-" + CatalogScanDriverType.LoadPackageArchive,
                "CatalogScan-" + CatalogScanDriverType.LoadPackageManifest,
            });
        }

        protected override IEnumerable<string> GetExpectedTableNames()
        {
            return base.GetExpectedTableNames().Concat(new[]
            {
                Options.Value.PackageArchiveTableName,
                Options.Value.PackageManifestTableName,
            });
        }
    }
}
