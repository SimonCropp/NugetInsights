﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Knapcode.ExplorePackages.Worker
{
    public abstract class BaseCatalogLeafScanToCsvIntegrationTest<T> : BaseCatalogScanToCsvIntegrationTest<T> where T : ICsvRecord
    {
        protected BaseCatalogLeafScanToCsvIntegrationTest(ITestOutputHelper output, DefaultWebApplicationFactory<StaticFilesStartup> factory) : base(output, factory)
        {
        }

        protected override Task<CatalogIndexScan> UpdateAsync(DateTimeOffset max)
        {
            return UpdateAsync(DriverType, LatestLeavesTypes.Contains(DriverType), max);
        }
    }

    public abstract class BaseCatalogLeafScanToCsvIntegrationTest<T1, T2> : BaseCatalogScanToCsvIntegrationTest<T1, T2>
        where T1 : ICsvRecord
        where T2 : ICsvRecord
    {
        protected BaseCatalogLeafScanToCsvIntegrationTest(ITestOutputHelper output, DefaultWebApplicationFactory<StaticFilesStartup> factory) : base(output, factory)
        {
        }

        protected override Task<CatalogIndexScan> UpdateAsync(DateTimeOffset max)
        {
            return UpdateAsync(DriverType, LatestLeavesTypes.Contains(DriverType), max);
        }
    }
}