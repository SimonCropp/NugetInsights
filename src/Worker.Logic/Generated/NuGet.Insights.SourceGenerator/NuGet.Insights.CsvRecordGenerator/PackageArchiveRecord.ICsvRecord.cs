﻿// <auto-generated />

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Insights;

namespace NuGet.Insights.Worker.PackageArchiveToCsv
{
    /* Kusto DDL:

    .drop table PackageArchives ifexists;

    .create table PackageArchives (
        LowerId: string,
        Identity: string,
        Id: string,
        Version: string,
        CatalogCommitTimestamp: datetime,
        Created: datetime,
        ResultType: string,
        Size: long,
        OffsetAfterEndOfCentralDirectory: long,
        CentralDirectorySize: long,
        OffsetOfCentralDirectory: long,
        EntryCount: int,
        Comment: string,
        HeaderMD5: string,
        HeaderSHA512: string,
        MD5: string,
        SHA1: string,
        SHA256: string,
        SHA512: string
    );

    .alter-merge table PackageArchives policy retention softdelete = 30d;

    .alter table PackageArchives policy partitioning '{'
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

    .create table PackageArchives ingestion csv mapping 'BlobStorageMapping'
    '['
        '{"Column":"LowerId","DataType":"string","Properties":{"Ordinal":2}},'
        '{"Column":"Identity","DataType":"string","Properties":{"Ordinal":3}},'
        '{"Column":"Id","DataType":"string","Properties":{"Ordinal":4}},'
        '{"Column":"Version","DataType":"string","Properties":{"Ordinal":5}},'
        '{"Column":"CatalogCommitTimestamp","DataType":"datetime","Properties":{"Ordinal":6}},'
        '{"Column":"Created","DataType":"datetime","Properties":{"Ordinal":7}},'
        '{"Column":"ResultType","DataType":"string","Properties":{"Ordinal":8}},'
        '{"Column":"Size","DataType":"long","Properties":{"Ordinal":9}},'
        '{"Column":"OffsetAfterEndOfCentralDirectory","DataType":"long","Properties":{"Ordinal":10}},'
        '{"Column":"CentralDirectorySize","DataType":"long","Properties":{"Ordinal":11}},'
        '{"Column":"OffsetOfCentralDirectory","DataType":"long","Properties":{"Ordinal":12}},'
        '{"Column":"EntryCount","DataType":"int","Properties":{"Ordinal":13}},'
        '{"Column":"Comment","DataType":"string","Properties":{"Ordinal":14}},'
        '{"Column":"HeaderMD5","DataType":"string","Properties":{"Ordinal":15}},'
        '{"Column":"HeaderSHA512","DataType":"string","Properties":{"Ordinal":16}},'
        '{"Column":"MD5","DataType":"string","Properties":{"Ordinal":17}},'
        '{"Column":"SHA1","DataType":"string","Properties":{"Ordinal":18}},'
        '{"Column":"SHA256","DataType":"string","Properties":{"Ordinal":19}},'
        '{"Column":"SHA512","DataType":"string","Properties":{"Ordinal":20}}'
    ']'

    */
    partial record PackageArchiveRecord
    {
        public int FieldCount => 21;

        public void WriteHeader(TextWriter writer)
        {
            writer.WriteLine("ScanId,ScanTimestamp,LowerId,Identity,Id,Version,CatalogCommitTimestamp,Created,ResultType,Size,OffsetAfterEndOfCentralDirectory,CentralDirectorySize,OffsetOfCentralDirectory,EntryCount,Comment,HeaderMD5,HeaderSHA512,MD5,SHA1,SHA256,SHA512");
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
            fields.Add(Size.ToString());
            fields.Add(OffsetAfterEndOfCentralDirectory.ToString());
            fields.Add(CentralDirectorySize.ToString());
            fields.Add(OffsetOfCentralDirectory.ToString());
            fields.Add(EntryCount.ToString());
            fields.Add(Comment);
            fields.Add(HeaderMD5);
            fields.Add(HeaderSHA512);
            fields.Add(MD5);
            fields.Add(SHA1);
            fields.Add(SHA256);
            fields.Add(SHA512);
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
            writer.Write(Size);
            writer.Write(',');
            writer.Write(OffsetAfterEndOfCentralDirectory);
            writer.Write(',');
            writer.Write(CentralDirectorySize);
            writer.Write(',');
            writer.Write(OffsetOfCentralDirectory);
            writer.Write(',');
            writer.Write(EntryCount);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Comment);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, HeaderMD5);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, HeaderSHA512);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, MD5);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, SHA1);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, SHA256);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, SHA512);
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
            await writer.WriteAsync(Size.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(OffsetAfterEndOfCentralDirectory.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(CentralDirectorySize.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(OffsetOfCentralDirectory.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(EntryCount.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Comment);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, HeaderMD5);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, HeaderSHA512);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, MD5);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, SHA1);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, SHA256);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, SHA512);
            await writer.WriteLineAsync();
        }

        public ICsvRecord ReadNew(Func<string> getNextField)
        {
            return new PackageArchiveRecord
            {
                ScanId = CsvUtility.ParseNullable(getNextField(), Guid.Parse),
                ScanTimestamp = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                LowerId = getNextField(),
                Identity = getNextField(),
                Id = getNextField(),
                Version = getNextField(),
                CatalogCommitTimestamp = CsvUtility.ParseDateTimeOffset(getNextField()),
                Created = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                ResultType = Enum.Parse<NuGet.Insights.Worker.ArchiveResultType>(getNextField()),
                Size = long.Parse(getNextField()),
                OffsetAfterEndOfCentralDirectory = long.Parse(getNextField()),
                CentralDirectorySize = uint.Parse(getNextField()),
                OffsetOfCentralDirectory = uint.Parse(getNextField()),
                EntryCount = int.Parse(getNextField()),
                Comment = getNextField(),
                HeaderMD5 = getNextField(),
                HeaderSHA512 = getNextField(),
                MD5 = getNextField(),
                SHA1 = getNextField(),
                SHA256 = getNextField(),
                SHA512 = getNextField(),
            };
        }
    }
}
