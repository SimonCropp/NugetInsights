﻿// <auto-generated />

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Insights;

namespace NuGet.Insights.Worker.PackageAssemblyToCsv
{
    /* Kusto DDL:

    .drop table PackageAssemblies ifexists;

    .create table PackageAssemblies (
        LowerId: string,
        Identity: string,
        Id: string,
        Version: string,
        CatalogCommitTimestamp: datetime,
        Created: datetime,
        ResultType: string,
        Path: string,
        FileName: string,
        FileExtension: string,
        TopLevelFolder: string,
        CompressedLength: long,
        EntryUncompressedLength: long,
        ActualUncompressedLength: long,
        FileSHA256: string,
        EdgeCases: string,
        AssemblyName: string,
        AssemblyVersion: string,
        Culture: string,
        PublicKeyToken: string,
        HashAlgorithm: string,
        HasPublicKey: bool,
        PublicKeyLength: int,
        PublicKeySHA1: string,
        CustomAttributes: dynamic,
        CustomAttributesFailedDecode: dynamic,
        CustomAttributesTotalCount: int,
        CustomAttributesTotalDataLength: int
    );

    .alter-merge table PackageAssemblies policy retention softdelete = 30d;

    .alter table PackageAssemblies policy partitioning '{'
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

    .create table PackageAssemblies ingestion csv mapping 'BlobStorageMapping'
    '['
        '{"Column":"LowerId","DataType":"string","Properties":{"Ordinal":2}},'
        '{"Column":"Identity","DataType":"string","Properties":{"Ordinal":3}},'
        '{"Column":"Id","DataType":"string","Properties":{"Ordinal":4}},'
        '{"Column":"Version","DataType":"string","Properties":{"Ordinal":5}},'
        '{"Column":"CatalogCommitTimestamp","DataType":"datetime","Properties":{"Ordinal":6}},'
        '{"Column":"Created","DataType":"datetime","Properties":{"Ordinal":7}},'
        '{"Column":"ResultType","DataType":"string","Properties":{"Ordinal":8}},'
        '{"Column":"Path","DataType":"string","Properties":{"Ordinal":9}},'
        '{"Column":"FileName","DataType":"string","Properties":{"Ordinal":10}},'
        '{"Column":"FileExtension","DataType":"string","Properties":{"Ordinal":11}},'
        '{"Column":"TopLevelFolder","DataType":"string","Properties":{"Ordinal":12}},'
        '{"Column":"CompressedLength","DataType":"long","Properties":{"Ordinal":13}},'
        '{"Column":"EntryUncompressedLength","DataType":"long","Properties":{"Ordinal":14}},'
        '{"Column":"ActualUncompressedLength","DataType":"long","Properties":{"Ordinal":15}},'
        '{"Column":"FileSHA256","DataType":"string","Properties":{"Ordinal":16}},'
        '{"Column":"EdgeCases","DataType":"string","Properties":{"Ordinal":17}},'
        '{"Column":"AssemblyName","DataType":"string","Properties":{"Ordinal":18}},'
        '{"Column":"AssemblyVersion","DataType":"string","Properties":{"Ordinal":19}},'
        '{"Column":"Culture","DataType":"string","Properties":{"Ordinal":20}},'
        '{"Column":"PublicKeyToken","DataType":"string","Properties":{"Ordinal":21}},'
        '{"Column":"HashAlgorithm","DataType":"string","Properties":{"Ordinal":22}},'
        '{"Column":"HasPublicKey","DataType":"bool","Properties":{"Ordinal":23}},'
        '{"Column":"PublicKeyLength","DataType":"int","Properties":{"Ordinal":24}},'
        '{"Column":"PublicKeySHA1","DataType":"string","Properties":{"Ordinal":25}},'
        '{"Column":"CustomAttributes","DataType":"dynamic","Properties":{"Ordinal":26}},'
        '{"Column":"CustomAttributesFailedDecode","DataType":"dynamic","Properties":{"Ordinal":27}},'
        '{"Column":"CustomAttributesTotalCount","DataType":"int","Properties":{"Ordinal":28}},'
        '{"Column":"CustomAttributesTotalDataLength","DataType":"int","Properties":{"Ordinal":29}}'
    ']'

    */
    partial record PackageAssembly
    {
        public int FieldCount => 30;

        public void WriteHeader(TextWriter writer)
        {
            writer.WriteLine("ScanId,ScanTimestamp,LowerId,Identity,Id,Version,CatalogCommitTimestamp,Created,ResultType,Path,FileName,FileExtension,TopLevelFolder,CompressedLength,EntryUncompressedLength,ActualUncompressedLength,FileSHA256,EdgeCases,AssemblyName,AssemblyVersion,Culture,PublicKeyToken,HashAlgorithm,HasPublicKey,PublicKeyLength,PublicKeySHA1,CustomAttributes,CustomAttributesFailedDecode,CustomAttributesTotalCount,CustomAttributesTotalDataLength");
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
            fields.Add(Path);
            fields.Add(FileName);
            fields.Add(FileExtension);
            fields.Add(TopLevelFolder);
            fields.Add(CompressedLength.ToString());
            fields.Add(EntryUncompressedLength.ToString());
            fields.Add(ActualUncompressedLength.ToString());
            fields.Add(FileSHA256);
            fields.Add(EdgeCases.ToString());
            fields.Add(AssemblyName);
            fields.Add(AssemblyVersion?.ToString());
            fields.Add(Culture);
            fields.Add(PublicKeyToken);
            fields.Add(HashAlgorithm.ToString());
            fields.Add(CsvUtility.FormatBool(HasPublicKey));
            fields.Add(PublicKeyLength.ToString());
            fields.Add(PublicKeySHA1);
            fields.Add(CustomAttributes);
            fields.Add(CustomAttributesFailedDecode);
            fields.Add(CustomAttributesTotalCount.ToString());
            fields.Add(CustomAttributesTotalDataLength.ToString());
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
            CsvUtility.WriteWithQuotes(writer, Path);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, FileName);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, FileExtension);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, TopLevelFolder);
            writer.Write(',');
            writer.Write(CompressedLength);
            writer.Write(',');
            writer.Write(EntryUncompressedLength);
            writer.Write(',');
            writer.Write(ActualUncompressedLength);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, FileSHA256);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, EdgeCases.ToString());
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, AssemblyName);
            writer.Write(',');
            writer.Write(AssemblyVersion);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Culture);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, PublicKeyToken);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, HashAlgorithm.ToString());
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(HasPublicKey));
            writer.Write(',');
            writer.Write(PublicKeyLength);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, PublicKeySHA1);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, CustomAttributes);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, CustomAttributesFailedDecode);
            writer.Write(',');
            writer.Write(CustomAttributesTotalCount);
            writer.Write(',');
            writer.Write(CustomAttributesTotalDataLength);
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
            await CsvUtility.WriteWithQuotesAsync(writer, Path);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, FileName);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, FileExtension);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, TopLevelFolder);
            await writer.WriteAsync(',');
            await writer.WriteAsync(CompressedLength.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(EntryUncompressedLength.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(ActualUncompressedLength.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, FileSHA256);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, EdgeCases.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, AssemblyName);
            await writer.WriteAsync(',');
            await writer.WriteAsync(AssemblyVersion?.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Culture);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, PublicKeyToken);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, HashAlgorithm.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(HasPublicKey));
            await writer.WriteAsync(',');
            await writer.WriteAsync(PublicKeyLength.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, PublicKeySHA1);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, CustomAttributes);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, CustomAttributesFailedDecode);
            await writer.WriteAsync(',');
            await writer.WriteAsync(CustomAttributesTotalCount.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(CustomAttributesTotalDataLength.ToString());
            await writer.WriteLineAsync();
        }

        public ICsvRecord ReadNew(Func<string> getNextField)
        {
            return new PackageAssembly
            {
                ScanId = CsvUtility.ParseNullable(getNextField(), Guid.Parse),
                ScanTimestamp = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                LowerId = getNextField(),
                Identity = getNextField(),
                Id = getNextField(),
                Version = getNextField(),
                CatalogCommitTimestamp = CsvUtility.ParseDateTimeOffset(getNextField()),
                Created = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                ResultType = Enum.Parse<PackageAssemblyResultType>(getNextField()),
                Path = getNextField(),
                FileName = getNextField(),
                FileExtension = getNextField(),
                TopLevelFolder = getNextField(),
                CompressedLength = CsvUtility.ParseNullable(getNextField(), long.Parse),
                EntryUncompressedLength = CsvUtility.ParseNullable(getNextField(), long.Parse),
                ActualUncompressedLength = CsvUtility.ParseNullable(getNextField(), long.Parse),
                FileSHA256 = getNextField(),
                EdgeCases = CsvUtility.ParseNullable(getNextField(), Enum.Parse<PackageAssemblyEdgeCases>),
                AssemblyName = getNextField(),
                AssemblyVersion = CsvUtility.ParseReference(getNextField(), System.Version.Parse),
                Culture = getNextField(),
                PublicKeyToken = getNextField(),
                HashAlgorithm = CsvUtility.ParseNullable(getNextField(), Enum.Parse<System.Reflection.AssemblyHashAlgorithm>),
                HasPublicKey = CsvUtility.ParseNullable(getNextField(), bool.Parse),
                PublicKeyLength = CsvUtility.ParseNullable(getNextField(), int.Parse),
                PublicKeySHA1 = getNextField(),
                CustomAttributes = getNextField(),
                CustomAttributesFailedDecode = getNextField(),
                CustomAttributesTotalCount = CsvUtility.ParseNullable(getNextField(), int.Parse),
                CustomAttributesTotalDataLength = CsvUtility.ParseNullable(getNextField(), int.Parse),
            };
        }
    }
}