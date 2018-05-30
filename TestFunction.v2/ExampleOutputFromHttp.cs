using Functions.Bindings.DataLakeStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace TestFunction.v2
{
    public static class ExampleOutputFromHttp
    {
        [FunctionName("ExampleOutputFromHttp")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            [DataLakeStore(AccountFQDN = @"fqdn", ApplicationId = @"applicationid", ClientSecret = @"clientsecret", TenantID = @"tentantid")]IAsyncCollector<DataLakeStoreOutput> items,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            items.AddAsync(new DataLakeStoreOutput()
            {
                filename = "/mydata/" + Guid.NewGuid().ToString() + ".txt",
                stream = req.Body
            });

            return new OkResult();
        }

    }
}
