// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Insights
{
    public class V2SearchResultItem
    {
        [JsonPropertyName("PackageRegistration")]
        public V2SearchResultPackageRegistration PackageRegistration { get; set; }

        [JsonPropertyName("Listed")]
        public bool Listed { get; set; }
    }
}
