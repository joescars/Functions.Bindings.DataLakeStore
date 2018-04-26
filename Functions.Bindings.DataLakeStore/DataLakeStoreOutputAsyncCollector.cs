using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.DataLake.Store;
using Microsoft.Azure.WebJobs;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using Newtonsoft.Json.Linq;

namespace Functions.Bindings.DataLakeStore
{

    public class DataLakeStoreOutputAsyncCollector : IAsyncCollector<DataLakeStoreOutput>
    {
        private static AdlsClient _adlsClient;
        private static ServiceClientCredentials _adlsCreds;
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

            if (item.filename == null)
            {
                throw new InvalidOperationException("You must specify a filename.");
            }

            _items.Add(item);            

            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Uri ADL_TOKEN_AUDIENCE = new Uri(@"https://datalake.azure.net/");

            // Generate crendetials
            var adlCreds = _adlsCreds ?? (_adlsCreds = GetCreds_SPI_SecretKey(_attribute.TenantID, ADL_TOKEN_AUDIENCE, _attribute.ApplicationId, _attribute.ClientSecret));

            // Create ADLS client object
            var adlsClient = _adlsClient ?? (_adlsClient = AdlsClient.CreateClient(_attribute.AccountFQDN, adlCreds));

            foreach (var item in _items)
            {
                // Create a file - automatically creates any parent directories that don't exist
                string fileName = item.filename;

                using (var stream = adlsClient.CreateFile(fileName, IfExists.Overwrite))
                {
                    item.stream.CopyTo(stream);
                }
            }

            return Task.CompletedTask;
        }

        private static ServiceClientCredentials GetCreds_SPI_SecretKey(
            string tenant,
            Uri tokenAudience,
            string clientId,
            string secretKey)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var serviceSettings = ActiveDirectoryServiceSettings.Azure;
            serviceSettings.TokenAudience = tokenAudience;

            var creds = ApplicationTokenProvider.LoginSilentAsync(
                tenant,
                clientId,
                secretKey,
                serviceSettings).GetAwaiter().GetResult();
            return creds;
        }
    }
}
