using Microsoft.Azure.DataLake.Store;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Functions.Bindings.DataLakeStore.Services
{
    public class DataLakeAdlsService
    {
        public static AdlsClient CreateAdlsClient(string tenant, string clientsecret, string applicationid, string fqdn)
        {
            Uri ADL_TOKEN_AUDIENCE = new Uri(@"https://datalake.azure.net/");

            var adlCreds = GetCreds_SPI_SecretKey(tenant, ADL_TOKEN_AUDIENCE, applicationid, clientsecret);

            return AdlsClient.CreateClient(fqdn, adlCreds);
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
