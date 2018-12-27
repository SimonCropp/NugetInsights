﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Knapcode.ExplorePackages.Entities;
using Knapcode.MiniZip;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NuGet.CatalogReader;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Knapcode.ExplorePackages.Logic
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExplorePackages(this IServiceCollection serviceCollection, ExplorePackagesSettings settings)
        {
            serviceCollection.AddMemoryCache();

            serviceCollection.AddDbContext<SqliteEntityContext>((x, dbContextOptionsBuilder) =>
            {
                var options = x.GetRequiredService<ExplorePackagesSettings>();
                dbContextOptionsBuilder
                    .UseSqlite(options.DatabaseConnectionString);
            }, contextLifetime: ServiceLifetime.Transient);
            serviceCollection.Remove(serviceCollection.Single(x => x.ServiceType == typeof(SqliteEntityContext)));
            serviceCollection.AddTransient<Func<bool, SqliteEntityContext>>(x => includeCommitCondition =>
            {
                if (includeCommitCondition)
                {
                    return new SqliteEntityContext(
                        x.GetRequiredService<ICommitCondition>(),
                        x.GetRequiredService<DbContextOptions<SqliteEntityContext>>());
                }
                else
                {
                    return new SqliteEntityContext(
                        NullCommitCondition.Instance,
                        x.GetRequiredService<DbContextOptions<SqliteEntityContext>>());
                }
            });

            serviceCollection.AddDbContext<SqlServerEntityContext>((x, dbContextOptionsBuilder) =>
            {
                var options = x.GetRequiredService<ExplorePackagesSettings>();
                dbContextOptionsBuilder
                    .UseSqlServer(options.DatabaseConnectionString);
            }, contextLifetime: ServiceLifetime.Transient);
            serviceCollection.Remove(serviceCollection.Single(x => x.ServiceType == typeof(SqlServerEntityContext)));
            serviceCollection.AddTransient<Func<bool, SqlServerEntityContext>>(x => includeCommitCondition =>
            {
                if (includeCommitCondition)
                {
                    return new SqlServerEntityContext(
                        x.GetRequiredService<ICommitCondition>(),
                        x.GetRequiredService<DbContextOptions<SqlServerEntityContext>>());
                }
                else
                {
                    return new SqlServerEntityContext(
                        NullCommitCondition.Instance,
                        x.GetRequiredService<DbContextOptions<SqlServerEntityContext>>());
                }
            });

            serviceCollection.AddTransient<Func<bool, IEntityContext>>(x => includeCommitCondition =>
            {
                var options = x.GetRequiredService<ExplorePackagesSettings>();
                switch (options.DatabaseType)
                {
                    case DatabaseType.Sqlite:
                        return x.GetRequiredService<Func<bool, SqliteEntityContext>>()(includeCommitCondition);
                    case DatabaseType.SqlServer:
                        return x.GetRequiredService<Func<bool, SqlServerEntityContext>>()(includeCommitCondition);
                    default:
                        throw new NotImplementedException($"The database type '{options.DatabaseType}' is not supported.");
                }
            });
            serviceCollection.AddTransient(x => x.GetRequiredService<Func<bool, IEntityContext>>()(true));
            serviceCollection.AddTransient<Func<IEntityContext>>(x => () => x.GetRequiredService<Func<bool, IEntityContext>>()(true));
            serviceCollection.AddTransient<EntityContextFactory>();
            serviceCollection.AddSingleton<ISingletonService>(x => new SingletonService(
                new LeaseService(
                    new EntityContextFactory(
                        () => x.GetRequiredService<Func<bool, IEntityContext>>()(false))),
                x.GetRequiredService<ILogger<SingletonService>>()));
            serviceCollection.AddTransient<ICommitCondition, LeaseCommitCondition>();

            serviceCollection.AddSingleton<UrlReporterProvider>();
            serviceCollection.AddTransient<UrlReporterHandler>();
            serviceCollection.AddTransient<LoggingHandler>();
            serviceCollection.AddSingleton(
                x => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                });
            serviceCollection.AddTransient(
                x => new InitializeServicePointHandler(
                    connectionLeaseTimeout: TimeSpan.FromMinutes(1)));
            serviceCollection.AddTransient<HttpMessageHandler>(
                x =>
                {
                    var httpClientHandler = x.GetRequiredService<HttpClientHandler>();
                    var initializeServicePointerHander = x.GetRequiredService<InitializeServicePointHandler>();
                    var urlReporterHandler = x.GetRequiredService<UrlReporterHandler>();

                    initializeServicePointerHander.InnerHandler = httpClientHandler;
                    urlReporterHandler.InnerHandler = initializeServicePointerHander;

                    return urlReporterHandler;
                });
            serviceCollection.AddSingleton(
                x =>
                {
                    var httpMessageHandler = x.GetRequiredService<HttpMessageHandler>();
                    var loggingHandler = x.GetRequiredService<LoggingHandler>();
                    loggingHandler.InnerHandler = httpMessageHandler;
                    var httpClient = new HttpClient(loggingHandler);
                    UserAgent.SetUserAgent(httpClient);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-ms-version", "2017-04-17");
                    return httpClient;
                });
            serviceCollection.AddSingleton(
                x =>
                {
                    var options = x.GetRequiredService<ExplorePackagesSettings>();
                    return new HttpSource(
                        new PackageSource(options.V3ServiceIndex),
                        () =>
                        {
                            var httpClientHandler = x.GetRequiredService<HttpClientHandler>();
                            var httpMessageHandler = x.GetRequiredService<HttpMessageHandler>();
                            return Task.FromResult<HttpHandlerResource>(new HttpHandlerResourceV3(
                                httpClientHandler,
                                httpMessageHandler));
                        },
                        NullThrottle.Instance);
                });

            var searchServiceUrlCache = new SearchServiceUrlCache();
            serviceCollection.AddSingleton(searchServiceUrlCache);
            serviceCollection.AddSingleton<ISearchServiceUrlCacheInvalidator>(searchServiceUrlCache);
            serviceCollection.AddSingleton(
                x => new CatalogReader(
                    new Uri(x.GetRequiredService<ExplorePackagesSettings>().V3ServiceIndex, UriKind.Absolute),
                    x.GetRequiredService<HttpSource>(),
                    cacheContext: null,
                    cacheTimeout: TimeSpan.Zero,
                    log: x.GetRequiredService<ILogger<CatalogReader>>().ToNuGetLogger()));

            serviceCollection.AddTransient(
                x => new HttpZipProvider(
                    x.GetRequiredService<HttpClient>())
                {
                    FirstBufferSize = 4096,
                    SecondBufferSize = 4096,
                    BufferGrowthExponent = 2,
                });
            serviceCollection.AddTransient<MZipFormat>();

            serviceCollection.AddTransient(x => settings.Clone());
            serviceCollection.AddTransient<NuspecStore>();
            serviceCollection.AddTransient<MZipStore>();
            serviceCollection.AddTransient<RemoteCursorService>();
            serviceCollection.AddTransient<IPortTester, PortTester>();
            serviceCollection.AddTransient<IPortDiscoverer, SimplePortDiscoverer>();
            serviceCollection.AddTransient<SearchServiceUrlDiscoverer>();
            serviceCollection.AddTransient<SearchServiceCursorReader>();
            serviceCollection.AddTransient<PackageQueryContextBuilder>();
            serviceCollection.AddTransient<IProgressReporter, NullProgressReporter>();
            serviceCollection.AddTransient<LatestV2PackageFetcher>();
            serviceCollection.AddTransient<LatestCatalogCommitFetcher>();
            serviceCollection.AddTransient<PackageBlobNameProvider>();
            serviceCollection.AddTransient<IFileStorageService, FileStorageService>();
            serviceCollection.AddTransient<IBlobStorageService, BlobStorageService>();
            serviceCollection.AddTransient<BlobStorageMigrator>();

            serviceCollection.AddTransient<PackageQueryProcessor>();
            serviceCollection.AddTransient<CatalogToDatabaseProcessor>();
            serviceCollection.AddTransient<CatalogToNuspecsProcessor>();
            serviceCollection.AddTransient<V2ToDatabaseProcessor>();
            serviceCollection.AddTransient<PackageDownloadsToDatabaseProcessor>();

            serviceCollection.AddTransient<MZipCommitProcessor>();
            serviceCollection.AddTransient<MZipCommitCollector>();
            serviceCollection.AddTransient<MZipToDatabaseCommitProcessor>();
            serviceCollection.AddTransient<MZipToDatabaseCommitCollector>();
            serviceCollection.AddTransient<DependenciesToDatabaseCommitProcessor>();
            serviceCollection.AddTransient<DependenciesToDatabaseCommitCollector>();
            serviceCollection.AddTransient<DependencyPackagesToDatabaseCommitProcessor>();
            serviceCollection.AddTransient<DependencyPackagesToDatabaseCommitCollector>();

            serviceCollection.AddTransient<PackageCommitEnumerator>();
            serviceCollection.AddTransient<ICommitEnumerator<PackageEntity>, PackageCommitEnumerator>();
            serviceCollection.AddTransient<ICommitEnumerator<PackageRegistrationEntity>, PackageRegistrationCommitEnumerator>();
            serviceCollection.AddTransient<CursorService>();
            serviceCollection.AddTransient<IETagService, ETagService>();
            serviceCollection.AddTransient<PackageService>();
            serviceCollection.AddTransient<IPackageService, PackageService>();
            serviceCollection.AddTransient<PackageQueryService>();
            serviceCollection.AddTransient<CatalogService>();
            serviceCollection.AddTransient<PackageDependencyService>();
            serviceCollection.AddTransient<ProblemService>();
            serviceCollection.AddTransient<ILeaseService, LeaseService>();

            serviceCollection.AddTransient<V2Parser>();
            serviceCollection.AddSingleton<ServiceIndexCache>();
            serviceCollection.AddTransient<GalleryClient>();
            serviceCollection.AddTransient<V2Client>();
            serviceCollection.AddTransient<PackagesContainerClient>();
            serviceCollection.AddTransient<FlatContainerClient>();
            serviceCollection.AddTransient<RegistrationClient>();
            serviceCollection.AddTransient<SearchClient>();
            serviceCollection.AddTransient<AutocompleteClient>();
            serviceCollection.AddTransient<IPackageDownloadsClient, PackageDownloadsClient>();
            serviceCollection.AddTransient<CatalogClient>();

            serviceCollection.AddTransient<GalleryConsistencyService>();
            serviceCollection.AddTransient<V2ConsistencyService>();
            serviceCollection.AddTransient<FlatContainerConsistencyService>();
            serviceCollection.AddTransient<PackagesContainerConsistencyService>();
            serviceCollection.AddTransient<RegistrationOriginalConsistencyService>();
            serviceCollection.AddTransient<RegistrationGzippedConsistencyService>();
            serviceCollection.AddTransient<RegistrationSemVer2ConsistencyService>();
            serviceCollection.AddTransient<SearchLoadBalancerConsistencyService>();
            serviceCollection.AddTransient<SearchSpecificInstancesConsistencyService>();
            serviceCollection.AddTransient<PackageConsistencyService>();
            serviceCollection.AddTransient<CrossCheckConsistencyService>();

            serviceCollection.AddTransient<FindIdsEndingInDotNumberNuspecQuery>();
            serviceCollection.AddTransient<FindRepositoriesNuspecQuery>();
            serviceCollection.AddTransient<FindInvalidDependencyVersionsNuspecQuery>();
            serviceCollection.AddTransient<FindMissingDependencyIdsNuspecQuery>();
            serviceCollection.AddTransient<FindPackageTypesNuspecQuery>();
            serviceCollection.AddTransient<FindSemVer2PackageVersionsNuspecQuery>();
            serviceCollection.AddTransient<FindSemVer2DependencyVersionsNuspecQuery>();
            serviceCollection.AddTransient<FindFloatingDependencyVersionsNuspecQuery>();
            serviceCollection.AddTransient<FindNonAsciiIdsNuspecQuery>();
            serviceCollection.AddTransient<FindInvalidPackageIdsNuspecQuery>();
            serviceCollection.AddTransient<FindInvalidPackageVersionsNuspecQuery>();
            serviceCollection.AddTransient<FindPackageVersionsContainingWhitespaceNuspecQuery>();
            serviceCollection.AddTransient<FindInvalidDependencyIdNuspecQuery>();
            serviceCollection.AddTransient<FindInvalidDependencyTargetFrameworkNuspecQuery>();
            serviceCollection.AddTransient<FindMixedDependencyGroupStylesNuspecQuery>();
            serviceCollection.AddTransient<FindWhitespaceDependencyTargetFrameworkNuspecQuery>();
            serviceCollection.AddTransient<FindUnsupportedDependencyTargetFrameworkNuspecQuery>();
            serviceCollection.AddTransient<FindDuplicateDependencyTargetFrameworksNuspecQuery>();
            serviceCollection.AddTransient<FindDuplicateNormalizedDependencyTargetFrameworksNuspecQuery>();
            serviceCollection.AddTransient<FindEmptyDependencyIdsNuspecQuery>();
            serviceCollection.AddTransient<FindWhitespaceDependencyIdsNuspecQuery>();
            serviceCollection.AddTransient<FindWhitespaceDependencyVersionsNuspecQuery>();
            serviceCollection.AddTransient<FindDuplicateDependenciesNuspecQuery>();
            serviceCollection.AddTransient<FindCaseSensitiveDuplicateMetadataElementsNuspecQuery>();
            serviceCollection.AddTransient<FindCaseInsensitiveDuplicateMetadataElementsNuspecQuery>();
            serviceCollection.AddTransient<FindCaseSensitiveDuplicateTextMetadataElementsNuspecQuery>();
            serviceCollection.AddTransient<FindCaseInsensitiveDuplicateTextMetadataElementsNuspecQuery>();
            serviceCollection.AddTransient<FindNonAlphabetMetadataElementsNuspecQuery>();
            serviceCollection.AddTransient<FindCollidingMetadataElementsNuspecQuery>();
            serviceCollection.AddTransient<FindUnexpectedValuesForBooleanMetadataNuspecQuery>();

            if (settings.RunBoringQueries)
            {
                serviceCollection.AddTransient<FindNonNormalizedPackageVersionsNuspecQuery>();
                serviceCollection.AddTransient<FindMissingDependencyVersionsNuspecQuery>();
                serviceCollection.AddTransient<FindEmptyDependencyVersionsNuspecQuery>();
            }

            if (settings.RunConsistencyChecks)
            {
                serviceCollection.AddTransient<IPackageQuery, HasV2DiscrepancyPackageQuery>();
                serviceCollection.AddTransient<IPackageQuery, HasPackagesContainerDiscrepancyPackageQuery>();
                serviceCollection.AddTransient<IPackageQuery, HasFlatContainerDiscrepancyPackageQuery>();
                serviceCollection.AddTransient<IPackageQuery, HasRegistrationDiscrepancyInOriginalHivePackageQuery>();
                serviceCollection.AddTransient<IPackageQuery, HasRegistrationDiscrepancyInGzippedHivePackageQuery>();
                serviceCollection.AddTransient<IPackageQuery, HasRegistrationDiscrepancyInSemVer2HivePackageQuery>();
                serviceCollection.AddTransient<IPackageQuery, HasSearchDiscrepancyPackageQuery>();
                serviceCollection.AddTransient<IPackageQuery, HasCrossCheckDiscrepancyPackageQuery>();
            }

            serviceCollection.AddTransient<IPackageQuery, HasMissingMZipPackageQuery>();
            serviceCollection.AddTransient<IPackageQuery, HasMissingNuspecPackageQuery>();

            // Add all of the .nuspec queries as package queries.
            var nuspecQueryDescriptors = serviceCollection
                .Where(x => typeof(INuspecQuery).IsAssignableFrom(x.ServiceType))
                .ToList();
            foreach (var nuspecQueryDescriptor in nuspecQueryDescriptors)
            {
                serviceCollection.AddTransient<IPackageQuery>(x =>
                {
                    var nuspecQuery = (INuspecQuery)x.GetRequiredService(nuspecQueryDescriptor.ImplementationType);
                    return new NuspecPackageQuery(nuspecQuery);
                });
            }

            return serviceCollection;
        }
    }
}
