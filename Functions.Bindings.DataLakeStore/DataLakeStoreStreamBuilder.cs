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
    public class DataLakeStoreStreamBuilder : IAsyncConverter<DataLakeStoreAttribute, Stream>
    {
        private static AdlsClient _adlsClient;
        private static ServiceClientCredentials _adlsCreds;
        private Stream _stream;

        private DataLakeStoreAttribute _attribute;

        public DataLakeStoreStreamBuilder(DataLakeConfiguration config)
        {
            
        }

        public async Task<Stream> ConvertAsync(DataLakeStoreAttribute input, CancellationToken cancellationToken)
        {
            Uri ADL_TOKEN_AUDIENCE = new Uri(@"https://datalake.azure.net/");

            // Generate crendetials
            var adlCreds = _adlsCreds ?? (_adlsCreds = GetCreds_SPI_SecretKey(input.TenantID, ADL_TOKEN_AUDIENCE, input.ApplicationId, input.ClientSecret));

            // Create ADLS client object
            var adlsClient = _adlsClient ?? (_adlsClient = AdlsClient.CreateClient(input.AccountFQDN, adlCreds));

            return await adlsClient.GetReadStreamAsync(input.FileName);
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
