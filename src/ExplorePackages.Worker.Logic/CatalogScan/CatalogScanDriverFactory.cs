﻿using System;
using Knapcode.ExplorePackages.Worker.FindCatalogLeafItem;
using Knapcode.ExplorePackages.Worker.FindLatestPackageLeaf;
using Knapcode.ExplorePackages.Worker.FindPackageAssembly;
using Knapcode.ExplorePackages.Worker.FindPackageAsset;
using Knapcode.ExplorePackages.Worker.FindPackageFile;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Knapcode.ExplorePackages.Worker
{
    public class CatalogScanDriverFactory : ICatalogScanDriverFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<ExplorePackagesWorkerSettings> _options;

        public CatalogScanDriverFactory(IServiceProvider serviceProvider, IOptions<ExplorePackagesWorkerSettings> options)
        {
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public ICatalogScanDriver Create(CatalogScanDriverType driverType)
        {
            return (ICatalogScanDriver)CreateBatchDriverOrNull(driverType) ?? CreateNonBatchDriver(driverType);
        }

        public ICatalogLeafScanBatchDriver CreateBatchDriverOrNull(CatalogScanDriverType driverType)
        {
            switch (driverType)
            {
                case CatalogScanDriverType.FindPackageFile:
                    return _serviceProvider.GetRequiredService<FindPackageFileDriver>();
                default:
                    if (_options.Value.RunAllCatalogScanDriversAsBatch)
                    {
                        return WrapNonBatchDriver(CreateNonBatchDriver(driverType));
                    }
                    else
                    {
                        return null;
                    }
            }
        }

        public ICatalogLeafScanNonBatchDriver CreateNonBatchDriver(CatalogScanDriverType driverType)
        {
            switch (driverType)
            {
                case CatalogScanDriverType.FindCatalogLeafItem:
                    return _serviceProvider.GetRequiredService<FindCatalogLeafItemDriver>();
                case CatalogScanDriverType.FindLatestCatalogLeafScan:
                    return _serviceProvider.GetRequiredService<FindLatestLeafDriver<CatalogLeafScan>>();
                case CatalogScanDriverType.FindLatestPackageLeaf:
                    return _serviceProvider.GetRequiredService<FindLatestLeafDriver<LatestPackageLeaf>>();
                case CatalogScanDriverType.FindPackageAssembly:
                    return _serviceProvider.GetRequiredService<CatalogLeafScanToCsvAdapter<PackageAssembly>>();
                case CatalogScanDriverType.FindPackageAsset:
                    return _serviceProvider.GetRequiredService<CatalogLeafScanToCsvAdapter<PackageAsset>>();
                default:
                    throw new NotSupportedException($"Catalog scan driver type '{driverType}' is not supported.");
            }
        }

        private ICatalogLeafScanBatchDriver WrapNonBatchDriver(ICatalogLeafScanNonBatchDriver driver)
        {
            return new CatalogLeafScanBatchDriverAdapter(
                driver,
                _serviceProvider.GetRequiredService<ILogger<CatalogLeafScanBatchDriverAdapter>>());
        }
    }
}
