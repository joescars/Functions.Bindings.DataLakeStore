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
using Functions.Bindings.DataLakeStore.Services;

namespace Functions.Bindings.DataLakeStore
{
    public class DataLakeStoreStreamBuilder : IAsyncConverter<DataLakeStoreAttribute, Stream>
    {
        private static AdlsClient _adlsClient;
        private DataLakeStoreAttribute _attribute;

        public DataLakeStoreStreamBuilder(DataLakeConfiguration config)
        {
            
        }

        public async Task<Stream> ConvertAsync(DataLakeStoreAttribute input, CancellationToken cancellationToken)
        {
            // Create ADLS client object
            var adlsClient = _adlsClient ?? (_adlsClient = DataLakeAdlsService.CreateAdlsClient(input.TenantID, input.ClientSecret, input.ApplicationId, input.AccountFQDN));

            return await adlsClient.GetReadStreamAsync(input.FileName);
        }
       
    }
}
