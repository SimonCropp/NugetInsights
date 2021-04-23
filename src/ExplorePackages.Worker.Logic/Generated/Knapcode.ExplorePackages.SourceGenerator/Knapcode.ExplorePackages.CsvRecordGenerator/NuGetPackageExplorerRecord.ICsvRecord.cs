﻿// <auto-generated />

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Knapcode.ExplorePackages;

namespace Knapcode.ExplorePackages.Worker.NuGetPackageExplorerToCsv
{
    /* Kusto DDL:

    .drop table JverNuGetPackageExplorers ifexists;

    .create table JverNuGetPackageExplorers (
        LowerId: string,
        Identity: string,
        Id: string,
        Version: string,
        CatalogCommitTimestamp: datetime,
        Created: datetime,
        ResultType: string,
        PackageSize: long,
        SourceLinkResult: string,
        DeterministicResult: string,
        CompilerFlagsResult: string,
        IsSignedByAuthor: bool,
        Name: string,
        Extension: string,
        TargetFramework: string,
        TargetFrameworkIdentifier: string,
        TargetFrameworkVersion: string,
        CompilerFlags: dynamic,
        HasCompilerFlags: bool,
        HasSourceLink: bool,
        HasDebugInfo: bool
    );

    .alter-merge table JverNuGetPackageExplorers policy retention softdelete = 30d;

    .alter table JverNuGetPackageExplorers policy partitioning '{'
      '"PartitionKeys": ['
        '{'
          '"ColumnName": "Identity",'
          '"Kind": "Hash",'
          '"Properties": {'
            '"Function": "XxHash64",'
            '"MaxPartitionCount": 256'
          '}'
        '}'
      ']'
    '}';

    .create table JverNuGetPackageExplorers ingestion csv mapping 'JverNuGetPackageExplorers_mapping'
    '['
        '{"Column":"LowerId","DataType":"string","Properties":{"Ordinal":2}},'
        '{"Column":"Identity","DataType":"string","Properties":{"Ordinal":3}},'
        '{"Column":"Id","DataType":"string","Properties":{"Ordinal":4}},'
        '{"Column":"Version","DataType":"string","Properties":{"Ordinal":5}},'
        '{"Column":"CatalogCommitTimestamp","DataType":"datetime","Properties":{"Ordinal":6}},'
        '{"Column":"Created","DataType":"datetime","Properties":{"Ordinal":7}},'
        '{"Column":"ResultType","DataType":"string","Properties":{"Ordinal":8}},'
        '{"Column":"PackageSize","DataType":"long","Properties":{"Ordinal":9}},'
        '{"Column":"SourceLinkResult","DataType":"string","Properties":{"Ordinal":10}},'
        '{"Column":"DeterministicResult","DataType":"string","Properties":{"Ordinal":11}},'
        '{"Column":"CompilerFlagsResult","DataType":"string","Properties":{"Ordinal":12}},'
        '{"Column":"IsSignedByAuthor","DataType":"bool","Properties":{"Ordinal":13}},'
        '{"Column":"Name","DataType":"string","Properties":{"Ordinal":14}},'
        '{"Column":"Extension","DataType":"string","Properties":{"Ordinal":15}},'
        '{"Column":"TargetFramework","DataType":"string","Properties":{"Ordinal":16}},'
        '{"Column":"TargetFrameworkIdentifier","DataType":"string","Properties":{"Ordinal":17}},'
        '{"Column":"TargetFrameworkVersion","DataType":"string","Properties":{"Ordinal":18}},'
        '{"Column":"CompilerFlags","DataType":"dynamic","Properties":{"Ordinal":19}},'
        '{"Column":"HasCompilerFlags","DataType":"bool","Properties":{"Ordinal":20}},'
        '{"Column":"HasSourceLink","DataType":"bool","Properties":{"Ordinal":21}},'
        '{"Column":"HasDebugInfo","DataType":"bool","Properties":{"Ordinal":22}}'
    ']'

    */
    partial record NuGetPackageExplorerRecord
    {
        public int FieldCount => 23;

        public void WriteHeader(TextWriter writer)
        {
            writer.WriteLine("ScanId,ScanTimestamp,LowerId,Identity,Id,Version,CatalogCommitTimestamp,Created,ResultType,PackageSize,SourceLinkResult,DeterministicResult,CompilerFlagsResult,IsSignedByAuthor,Name,Extension,TargetFramework,TargetFrameworkIdentifier,TargetFrameworkVersion,CompilerFlags,HasCompilerFlags,HasSourceLink,HasDebugInfo");
        }

        public void Write(List<string> fields)
        {
            fields.Add(ScanId.ToString());
            fields.Add(CsvUtility.FormatDateTimeOffset(ScanTimestamp));
            fields.Add(LowerId);
            fields.Add(Identity);
            fields.Add(Id);
            fields.Add(Version);
            fields.Add(CsvUtility.FormatDateTimeOffset(CatalogCommitTimestamp));
            fields.Add(CsvUtility.FormatDateTimeOffset(Created));
            fields.Add(ResultType.ToString());
            fields.Add(PackageSize.ToString());
            fields.Add(SourceLinkResult.ToString());
            fields.Add(DeterministicResult.ToString());
            fields.Add(CompilerFlagsResult.ToString());
            fields.Add(CsvUtility.FormatBool(IsSignedByAuthor));
            fields.Add(Name);
            fields.Add(Extension);
            fields.Add(TargetFramework);
            fields.Add(TargetFrameworkIdentifier);
            fields.Add(TargetFrameworkVersion);
            fields.Add(CompilerFlags);
            fields.Add(CsvUtility.FormatBool(HasCompilerFlags));
            fields.Add(CsvUtility.FormatBool(HasSourceLink));
            fields.Add(CsvUtility.FormatBool(HasDebugInfo));
        }

        public void Write(TextWriter writer)
        {
            writer.Write(ScanId);
            writer.Write(',');
            writer.Write(CsvUtility.FormatDateTimeOffset(ScanTimestamp));
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, LowerId);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Identity);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Id);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Version);
            writer.Write(',');
            writer.Write(CsvUtility.FormatDateTimeOffset(CatalogCommitTimestamp));
            writer.Write(',');
            writer.Write(CsvUtility.FormatDateTimeOffset(Created));
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, ResultType.ToString());
            writer.Write(',');
            writer.Write(PackageSize);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, SourceLinkResult.ToString());
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, DeterministicResult.ToString());
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, CompilerFlagsResult.ToString());
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(IsSignedByAuthor));
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Name);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Extension);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, TargetFramework);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, TargetFrameworkIdentifier);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, TargetFrameworkVersion);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, CompilerFlags);
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(HasCompilerFlags));
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(HasSourceLink));
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(HasDebugInfo));
            writer.WriteLine();
        }

        public async Task WriteAsync(TextWriter writer)
        {
            await writer.WriteAsync(ScanId.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatDateTimeOffset(ScanTimestamp));
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, LowerId);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Identity);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Id);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Version);
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatDateTimeOffset(CatalogCommitTimestamp));
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatDateTimeOffset(Created));
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, ResultType.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(PackageSize.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, SourceLinkResult.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, DeterministicResult.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, CompilerFlagsResult.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(IsSignedByAuthor));
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Name);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Extension);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, TargetFramework);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, TargetFrameworkIdentifier);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, TargetFrameworkVersion);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, CompilerFlags);
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(HasCompilerFlags));
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(HasSourceLink));
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(HasDebugInfo));
            await writer.WriteLineAsync();
        }

        public NuGetPackageExplorerRecord Read(Func<string> getNextField)
        {
            return new NuGetPackageExplorerRecord
            {
                ScanId = CsvUtility.ParseNullable(getNextField(), Guid.Parse),
                ScanTimestamp = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                LowerId = getNextField(),
                Identity = getNextField(),
                Id = getNextField(),
                Version = getNextField(),
                CatalogCommitTimestamp = CsvUtility.ParseDateTimeOffset(getNextField()),
                Created = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                ResultType = Enum.Parse<NuGetPackageExplorerResultType>(getNextField()),
                PackageSize = long.Parse(getNextField()),
                SourceLinkResult = Enum.Parse<NuGetPe.SymbolValidationResult>(getNextField()),
                DeterministicResult = Enum.Parse<NuGetPe.DeterministicResult>(getNextField()),
                CompilerFlagsResult = Enum.Parse<NuGetPe.HasCompilerFlagsResult>(getNextField()),
                IsSignedByAuthor = bool.Parse(getNextField()),
                Name = getNextField(),
                Extension = getNextField(),
                TargetFramework = getNextField(),
                TargetFrameworkIdentifier = getNextField(),
                TargetFrameworkVersion = getNextField(),
                CompilerFlags = getNextField(),
                HasCompilerFlags = CsvUtility.ParseNullable(getNextField(), bool.Parse),
                HasSourceLink = CsvUtility.ParseNullable(getNextField(), bool.Parse),
                HasDebugInfo = CsvUtility.ParseNullable(getNextField(), bool.Parse),
            };
        }
    }
}
