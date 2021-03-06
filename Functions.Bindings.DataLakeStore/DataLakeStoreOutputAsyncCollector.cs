﻿using Functions.Bindings.DataLakeStore.Services;
using Microsoft.Azure.DataLake.Store;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Functions.Bindings.DataLakeStore
{

    internal class DataLakeStoreOutputAsyncCollector : IAsyncCollector<DataLakeStoreOutput>
    {
        private static AdlsClient _adlsClient;
        private readonly Collection<DataLakeStoreOutput> _items = new Collection<DataLakeStoreOutput>();

        private DataLakeStoreAttribute _attribute;

        public DataLakeStoreOutputAsyncCollector(DataLakeStoreAttribute attr)
        {
            this._attribute = attr;
        }

        public async Task AddAsync(DataLakeStoreOutput item, CancellationToken cancellationToken = default(CancellationToken))
        {

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (item.FileName == null)
            {
                throw new InvalidOperationException("You must specify a filename.");
            }

            // Create ADLS Client
            var adlsClient = _adlsClient ?? (_adlsClient = await DataLakeAdlsService.CreateAdlsClientAsync(
                _attribute.TenantID, _attribute.ClientSecret, _attribute.ApplicationId, _attribute.AccountFQDN
                ));

            // Write the file
            using (var stream = await adlsClient.CreateFileAsync(item.FileName, IfExists.Overwrite))
            {
                await item.FileStream.CopyToAsync(stream);
            }
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

    }
}
