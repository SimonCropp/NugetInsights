﻿// <auto-generated />

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Insights;

namespace NuGet.Insights.Worker.PackageManifestToCsv
{
    /* Kusto DDL:

    .drop table PackageManifests ifexists;

    .create table PackageManifests (
        LowerId: string,
        Identity: string,
        Id: string,
        Version: string,
        CatalogCommitTimestamp: datetime,
        Created: datetime,
        ResultType: string,
        Size: int,
        OriginalId: string,
        OriginalVersion: string,
        MinClientVersion: string,
        DevelopmentDependency: bool,
        IsServiceable: bool,
        Authors: string,
        Copyright: string,
        Description: string,
        Icon: string,
        IconUrl: string,
        Language: string,
        LicenseUrl: string,
        Owners: string,
        ProjectUrl: string,
        Readme: string,
        ReleaseNotes: string,
        RequireLicenseAcceptance: bool,
        Summary: string,
        Tags: string,
        Title: string,
        PackageTypes: dynamic,
        LicenseMetadata: dynamic,
        RepositoryMetadata: dynamic,
        ReferenceGroups: dynamic,
        ContentFiles: dynamic,
        DependencyGroups: dynamic,
        FrameworkAssemblyGroups: dynamic,
        FrameworkRefGroups: dynamic,
        ContentFilesHasFormatException: bool,
        DependencyGroupsHasMissingId: bool,
        SplitTags: dynamic
    ) with (docstring = "See https://github.com/NuGet/Insights/blob/main/docs/tables/PackageManifests.md", folder = "");

    .alter-merge table PackageManifests policy retention softdelete = 30d;

    .alter table PackageManifests policy partitioning '{'
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

    .create table PackageManifests ingestion csv mapping 'BlobStorageMapping'
    '['
        '{"Column":"LowerId","DataType":"string","Properties":{"Ordinal":2}},'
        '{"Column":"Identity","DataType":"string","Properties":{"Ordinal":3}},'
        '{"Column":"Id","DataType":"string","Properties":{"Ordinal":4}},'
        '{"Column":"Version","DataType":"string","Properties":{"Ordinal":5}},'
        '{"Column":"CatalogCommitTimestamp","DataType":"datetime","Properties":{"Ordinal":6}},'
        '{"Column":"Created","DataType":"datetime","Properties":{"Ordinal":7}},'
        '{"Column":"ResultType","DataType":"string","Properties":{"Ordinal":8}},'
        '{"Column":"Size","DataType":"int","Properties":{"Ordinal":9}},'
        '{"Column":"OriginalId","DataType":"string","Properties":{"Ordinal":10}},'
        '{"Column":"OriginalVersion","DataType":"string","Properties":{"Ordinal":11}},'
        '{"Column":"MinClientVersion","DataType":"string","Properties":{"Ordinal":12}},'
        '{"Column":"DevelopmentDependency","DataType":"bool","Properties":{"Ordinal":13}},'
        '{"Column":"IsServiceable","DataType":"bool","Properties":{"Ordinal":14}},'
        '{"Column":"Authors","DataType":"string","Properties":{"Ordinal":15}},'
        '{"Column":"Copyright","DataType":"string","Properties":{"Ordinal":16}},'
        '{"Column":"Description","DataType":"string","Properties":{"Ordinal":17}},'
        '{"Column":"Icon","DataType":"string","Properties":{"Ordinal":18}},'
        '{"Column":"IconUrl","DataType":"string","Properties":{"Ordinal":19}},'
        '{"Column":"Language","DataType":"string","Properties":{"Ordinal":20}},'
        '{"Column":"LicenseUrl","DataType":"string","Properties":{"Ordinal":21}},'
        '{"Column":"Owners","DataType":"string","Properties":{"Ordinal":22}},'
        '{"Column":"ProjectUrl","DataType":"string","Properties":{"Ordinal":23}},'
        '{"Column":"Readme","DataType":"string","Properties":{"Ordinal":24}},'
        '{"Column":"ReleaseNotes","DataType":"string","Properties":{"Ordinal":25}},'
        '{"Column":"RequireLicenseAcceptance","DataType":"bool","Properties":{"Ordinal":26}},'
        '{"Column":"Summary","DataType":"string","Properties":{"Ordinal":27}},'
        '{"Column":"Tags","DataType":"string","Properties":{"Ordinal":28}},'
        '{"Column":"Title","DataType":"string","Properties":{"Ordinal":29}},'
        '{"Column":"PackageTypes","DataType":"dynamic","Properties":{"Ordinal":30}},'
        '{"Column":"LicenseMetadata","DataType":"dynamic","Properties":{"Ordinal":31}},'
        '{"Column":"RepositoryMetadata","DataType":"dynamic","Properties":{"Ordinal":32}},'
        '{"Column":"ReferenceGroups","DataType":"dynamic","Properties":{"Ordinal":33}},'
        '{"Column":"ContentFiles","DataType":"dynamic","Properties":{"Ordinal":34}},'
        '{"Column":"DependencyGroups","DataType":"dynamic","Properties":{"Ordinal":35}},'
        '{"Column":"FrameworkAssemblyGroups","DataType":"dynamic","Properties":{"Ordinal":36}},'
        '{"Column":"FrameworkRefGroups","DataType":"dynamic","Properties":{"Ordinal":37}},'
        '{"Column":"ContentFilesHasFormatException","DataType":"bool","Properties":{"Ordinal":38}},'
        '{"Column":"DependencyGroupsHasMissingId","DataType":"bool","Properties":{"Ordinal":39}},'
        '{"Column":"SplitTags","DataType":"dynamic","Properties":{"Ordinal":40}}'
    ']'

    */
    partial record PackageManifestRecord
    {
        public int FieldCount => 41;

        public void WriteHeader(TextWriter writer)
        {
            writer.WriteLine("ScanId,ScanTimestamp,LowerId,Identity,Id,Version,CatalogCommitTimestamp,Created,ResultType,Size,OriginalId,OriginalVersion,MinClientVersion,DevelopmentDependency,IsServiceable,Authors,Copyright,Description,Icon,IconUrl,Language,LicenseUrl,Owners,ProjectUrl,Readme,ReleaseNotes,RequireLicenseAcceptance,Summary,Tags,Title,PackageTypes,LicenseMetadata,RepositoryMetadata,ReferenceGroups,ContentFiles,DependencyGroups,FrameworkAssemblyGroups,FrameworkRefGroups,ContentFilesHasFormatException,DependencyGroupsHasMissingId,SplitTags");
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
            fields.Add(OriginalId);
            fields.Add(OriginalVersion);
            fields.Add(MinClientVersion);
            fields.Add(CsvUtility.FormatBool(DevelopmentDependency));
            fields.Add(CsvUtility.FormatBool(IsServiceable));
            fields.Add(Authors);
            fields.Add(Copyright);
            fields.Add(Description);
            fields.Add(Icon);
            fields.Add(IconUrl);
            fields.Add(Language);
            fields.Add(LicenseUrl);
            fields.Add(Owners);
            fields.Add(ProjectUrl);
            fields.Add(Readme);
            fields.Add(ReleaseNotes);
            fields.Add(CsvUtility.FormatBool(RequireLicenseAcceptance));
            fields.Add(Summary);
            fields.Add(Tags);
            fields.Add(Title);
            fields.Add(PackageTypes);
            fields.Add(LicenseMetadata);
            fields.Add(RepositoryMetadata);
            fields.Add(ReferenceGroups);
            fields.Add(ContentFiles);
            fields.Add(DependencyGroups);
            fields.Add(FrameworkAssemblyGroups);
            fields.Add(FrameworkRefGroups);
            fields.Add(CsvUtility.FormatBool(ContentFilesHasFormatException));
            fields.Add(CsvUtility.FormatBool(DependencyGroupsHasMissingId));
            fields.Add(SplitTags);
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
            CsvUtility.WriteWithQuotes(writer, OriginalId);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, OriginalVersion);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, MinClientVersion);
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(DevelopmentDependency));
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(IsServiceable));
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Authors);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Copyright);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Description);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Icon);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, IconUrl);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Language);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, LicenseUrl);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Owners);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, ProjectUrl);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Readme);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, ReleaseNotes);
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(RequireLicenseAcceptance));
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Summary);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Tags);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, Title);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, PackageTypes);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, LicenseMetadata);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, RepositoryMetadata);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, ReferenceGroups);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, ContentFiles);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, DependencyGroups);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, FrameworkAssemblyGroups);
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, FrameworkRefGroups);
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(ContentFilesHasFormatException));
            writer.Write(',');
            writer.Write(CsvUtility.FormatBool(DependencyGroupsHasMissingId));
            writer.Write(',');
            CsvUtility.WriteWithQuotes(writer, SplitTags);
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
            await CsvUtility.WriteWithQuotesAsync(writer, OriginalId);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, OriginalVersion);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, MinClientVersion);
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(DevelopmentDependency));
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(IsServiceable));
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Authors);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Copyright);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Description);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Icon);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, IconUrl);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Language);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, LicenseUrl);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Owners);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, ProjectUrl);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Readme);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, ReleaseNotes);
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(RequireLicenseAcceptance));
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Summary);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Tags);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, Title);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, PackageTypes);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, LicenseMetadata);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, RepositoryMetadata);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, ReferenceGroups);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, ContentFiles);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, DependencyGroups);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, FrameworkAssemblyGroups);
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, FrameworkRefGroups);
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(ContentFilesHasFormatException));
            await writer.WriteAsync(',');
            await writer.WriteAsync(CsvUtility.FormatBool(DependencyGroupsHasMissingId));
            await writer.WriteAsync(',');
            await CsvUtility.WriteWithQuotesAsync(writer, SplitTags);
            await writer.WriteLineAsync();
        }

        public ICsvRecord ReadNew(Func<string> getNextField)
        {
            return new PackageManifestRecord
            {
                ScanId = CsvUtility.ParseNullable(getNextField(), Guid.Parse),
                ScanTimestamp = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                LowerId = getNextField(),
                Identity = getNextField(),
                Id = getNextField(),
                Version = getNextField(),
                CatalogCommitTimestamp = CsvUtility.ParseDateTimeOffset(getNextField()),
                Created = CsvUtility.ParseNullable(getNextField(), CsvUtility.ParseDateTimeOffset),
                ResultType = Enum.Parse<PackageManifestRecordResultType>(getNextField()),
                Size = int.Parse(getNextField()),
                OriginalId = getNextField(),
                OriginalVersion = getNextField(),
                MinClientVersion = getNextField(),
                DevelopmentDependency = bool.Parse(getNextField()),
                IsServiceable = bool.Parse(getNextField()),
                Authors = getNextField(),
                Copyright = getNextField(),
                Description = getNextField(),
                Icon = getNextField(),
                IconUrl = getNextField(),
                Language = getNextField(),
                LicenseUrl = getNextField(),
                Owners = getNextField(),
                ProjectUrl = getNextField(),
                Readme = getNextField(),
                ReleaseNotes = getNextField(),
                RequireLicenseAcceptance = bool.Parse(getNextField()),
                Summary = getNextField(),
                Tags = getNextField(),
                Title = getNextField(),
                PackageTypes = getNextField(),
                LicenseMetadata = getNextField(),
                RepositoryMetadata = getNextField(),
                ReferenceGroups = getNextField(),
                ContentFiles = getNextField(),
                DependencyGroups = getNextField(),
                FrameworkAssemblyGroups = getNextField(),
                FrameworkRefGroups = getNextField(),
                ContentFilesHasFormatException = bool.Parse(getNextField()),
                DependencyGroupsHasMissingId = bool.Parse(getNextField()),
                SplitTags = getNextField(),
            };
        }
    }
}
