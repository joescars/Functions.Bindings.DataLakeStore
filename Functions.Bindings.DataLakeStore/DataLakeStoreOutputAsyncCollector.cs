using Functions.Bindings.DataLakeStore.Services;
using Microsoft.Azure.DataLake.Store;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Functions.Bindings.DataLakeStore
{

    public class DataLakeStoreOutputAsyncCollector : IAsyncCollector<DataLakeStoreOutput>
    {
        private static AdlsClient _adlsClient;
        private readonly Collection<DataLakeStoreOutput> _items = new Collection<DataLakeStoreOutput>();

        private DataLakeStoreAttribute _attribute;

        public DataLakeStoreOutputAsyncCollector(DataLakeStoreAttribute attr)
        {
            this._attribute = attr;
        }

        public Task AddAsync(DataLakeStoreOutput item, CancellationToken cancellationToken = default(CancellationToken))
        {

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (item.FileName == null)
            {
                throw new InvalidOperationException("You must specify a filename.");
            }

            _items.Add(item);            

            return Task.CompletedTask;
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            // Create ADLS client object
            var adlsClient = _adlsClient ?? (_adlsClient =  await DataLakeAdlsService.CreateAdlsClientAsync(_attribute.TenantID, _attribute.ClientSecret, _attribute.ApplicationId, _attribute.AccountFQDN));

            foreach (var item in _items)
            {
                // Create a file - automatically creates any parent directories that don't exist
                string fileName = item.FileName;

                using (var stream = adlsClient.CreateFile(fileName, IfExists.Overwrite))
                {
                    item.FileStream.CopyTo(stream);
                }
            }

        }

    }
}
