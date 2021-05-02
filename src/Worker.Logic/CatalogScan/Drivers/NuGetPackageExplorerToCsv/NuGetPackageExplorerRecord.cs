﻿using System;
using NuGetPe;

namespace Knapcode.ExplorePackages.Worker.NuGetPackageExplorerToCsv
{
    public partial record NuGetPackageExplorerRecord : PackageRecord, ICsvRecord
    {
        public NuGetPackageExplorerRecord()
        {
        }

        public NuGetPackageExplorerRecord(Guid scanId, DateTimeOffset scanTimestamp, PackageDeleteCatalogLeaf leaf)
            : base(scanId, scanTimestamp, leaf)
        {
            ResultType = NuGetPackageExplorerResultType.Deleted;
        }

        public NuGetPackageExplorerRecord(Guid scanId, DateTimeOffset scanTimestamp, PackageDetailsCatalogLeaf leaf)
            : base(scanId, scanTimestamp, leaf)
        {
            ResultType = NuGetPackageExplorerResultType.Available;
        }

        public NuGetPackageExplorerResultType ResultType { get; set; }

        public SymbolValidationResult? SourceLinkResult { get; set; }
        public DeterministicResult? DeterministicResult { get; set; }
        public HasCompilerFlagsResult? CompilerFlagsResult { get; set; }
        public bool? IsSignedByAuthor { get; set; }
    }
}

#if !ENABLE_NPE
namespace NuGetPe
{
    public enum SymbolValidationResult
    {
    }
    public enum DeterministicResult
    {
    }
    public enum HasCompilerFlagsResult
    {
    }
}
#endif