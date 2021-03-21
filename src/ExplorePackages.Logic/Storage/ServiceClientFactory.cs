﻿using System;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;

namespace Knapcode.ExplorePackages
{
    public class ServiceClientFactory
    {
        private readonly Lazy<CloudStorageAccount> _storageAccount;

        public ServiceClientFactory(IOptions<ExplorePackagesSettings> options)
        {
            _storageAccount = GetLazyStorageAccount(options.Value.StorageConnectionString);
        }

        public CloudStorageAccount GetStorageAccount()
        {
            return _storageAccount.Value;
        }

        private static Lazy<CloudStorageAccount> GetLazyStorageAccount(string connectionString)
        {
            return new Lazy<CloudStorageAccount>(() => CloudStorageAccount.Parse(connectionString));
        }
    }
}
