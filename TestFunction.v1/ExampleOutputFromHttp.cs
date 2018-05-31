using Functions.Bindings.DataLakeStore;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestFunction.v1
{
    public static class ExampleOutputFromHttp
    {
        [FunctionName("ExampleOutputFromHttp")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            [DataLakeStore(AccountFQDN = @"fqdn", ApplicationId = @"applicationid", ClientSecret = @"clientsecret", TenantID = @"tentantid")]IAsyncCollector<DataLakeStoreOutput> items,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            await items.AddAsync(new DataLakeStoreOutput()
            {
                FileName = "/mydata/" + Guid.NewGuid().ToString() + ".txt",
                FileStream = await req.Content.ReadAsStreamAsync()
            });

            return req.CreateResponse(HttpStatusCode.OK);
            
        }
    }
}
