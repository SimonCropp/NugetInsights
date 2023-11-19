﻿// <auto-generated />

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Insights;

namespace NuGet.Insights.Worker.PackageContentToCsv
{
    /* Kusto DDL:

    .drop table PackageContents ifexists;

    .create table PackageContents (
        LowerId: string,
        Identity: string,
        Id: string,
        Version: string,
        CatalogCommitTimestamp: datetime,
        Created: datetime,
        ResultType: string,
        Path: string,
        FileExtension: string,
        SequenceNumber: int,
        Size: int,
        Truncated: bool,
        TruncatedSize: int,
        SHA256: string,
        Content: string,
        DuplicateContent: bool
    ) with (docstring = "See https://github.com/NuGet/Insights/blob/main/docs/tables/PackageContents.md", folder = "");

    .alter-merge table PackageContents policy retention softdelete = 30d;

    .alter table PackageContents policy partitioning '{'
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

    .create table PackageContents ingestion csv mapping 'BlobStorageMapping'
    '['
        '{"Column":"LowerId","DataType":"string","Properties":{"Ordinal":2}},'
        '{"Column":"Identity","DataType":"string","Properties":{"Ordinal":3}},'
        '{"Column":"Id","DataType":"string","Properties":{"Ordinal":4}},'
        '{"Column":"Version","DataType":"string","Properties":{"Ordinal":5}},'
        '{"Column":"CatalogCommitTimestamp","DataType":"datetime","Properties":{"Ordinal":6}},'
        '{"Column":"Created","DataType":"datetime","Properties":{"Ordinal":7}},'
        '{"Column":"ResultType","DataType":"string","Properties":{"Ordinal":8}},'
        '{"Column":"Path","DataType":"string","Properties":{"Ordinal":9}},'
        '{"Column":"FileExtension","DataType":"string","Properties":{"Ordinal":10}},'
        '{"Column":"SequenceNumber","DataType":"int","Properties":{"Ordinal":11}},'
        '{"Column":"Size","DataType":"int","Properties":{"Ordinal":12}},'
        '{"Column":"Truncated","DataType":"bool","Properties":{"Ordinal":13}},'
        '{"Column":"TruncatedSize","DataType":"int","Properties":{"Ordinal":14}},'
        '{"Column":"SHA256","DataType":"string","Properties":{"Ordinal":15}},'
        '{"Column":"Content","DataType":"string","Properties":{"Ordinal":16}},'
        '{"Column":"DuplicateContent","DataType":"bool","Properties":{"Ordinal":17}}'
    ']'

    */
    partial record PackageContent
    {
        public int FieldCount => 18;

        public void WriteHeader(TextWriter writer)
        {
            writer.WriteLine("ScanId,ScanTimestamp,LowerId,Identity,Id,Version,CatalogCommitTimestamp,Created,ResultType,Path,FileExtension,SequenceNumber,Size,Truncated,TruncatedSize,SHA256,Content,DuplicateContent");
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
            fields.Add(FileExtension);
            fields.Add(SequenceNumber.ToString());
            fields.Add(Size.ToString());
            fields.Add(CsvUtility.FormatBool(Truncated));
            fields.Add(TruncatedSize.ToString());
            fields.Add(SHA256);
            fields.Add(Content);
            fields.Add(CsvUtility.FormatBool(DuplicateContent));
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
            CsvUtility.WriteWithQuotes(writer, FileExtension);
            writer.Write(',');
            writer.Write(SequenceNumber);
            writer.Write(',');
            writer.Write(Size);
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(Truncated));
            writer.Write(',');
            writer.Write(TruncatedSize);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, SHA256);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Content);
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(DuplicateContent));
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
            await CsvUtility.WriteWithQuotesAsync(writer, FileExtension);
            await writer.WriteAsync(',');
            await writer.WriteAsync(SequenceNumber.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(Size.ToString());
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(Truncated));
            await writer.WriteAsync(',');
            await writer.WriteAsync(TruncatedSize.ToString());
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, SHA256);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Content);
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(DuplicateContent));
            await writer.WriteLineAsync();
        }

        public ICsvRecord ReadNew(Func<string> getNextField)
        {
            return new PackageContent
            {
                ScanId = CsvUtility.ParseNullable(getNextField(), Guid.Parse),
                ScanTimestamp = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                LowerId = getNextField(),
                Identity = getNextField(),
                Id = getNextField(),
                Version = getNextField(),
                CatalogCommitTimestamp = CsvUtility.ParseDateTimeOffset(getNextField()),
                Created = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                ResultType = Enum.Parse<PackageContentResultType>(getNextField()),
                Path = getNextField(),
                FileExtension = getNextField(),
                SequenceNumber = CsvUtility.ParseNullable(getNextField(), int.Parse),
                Size = CsvUtility.ParseNullable(getNextField(), int.Parse),
                Truncated = CsvUtility.ParseNullable(getNextField(), bool.Parse),
                TruncatedSize = CsvUtility.ParseNullable(getNextField(), int.Parse),
                SHA256 = getNextField(),
                Content = getNextField(),
                DuplicateContent = CsvUtility.ParseNullable(getNextField(), bool.Parse),
            };
        }

        public void SetEmptyStrings()
        {
            if (LowerId is null)
            {
                LowerId = string.Empty;
            }

            if (Identity is null)
            {
                Identity = string.Empty;
            }

            if (Id is null)
            {
                Id = string.Empty;
            }

            if (Version is null)
            {
                Version = string.Empty;
            }

            if (Path is null)
            {
                Path = string.Empty;
            }

            if (FileExtension is null)
            {
                FileExtension = string.Empty;
            }

            if (SHA256 is null)
            {
                SHA256 = string.Empty;
            }

            if (Content is null)
            {
                Content = string.Empty;
            }
        }
    }
}
