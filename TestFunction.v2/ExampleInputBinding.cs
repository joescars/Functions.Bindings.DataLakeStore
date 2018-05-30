using Functions.Bindings.DataLakeStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.IO;

namespace TestFunction.v2
{
    public static class ExampleInputBinding
    {
        [FunctionName("ExampleInputBinding")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            [DataLakeStore(AccountFQDN = @"fqdn", ApplicationId = @"applicationid", ClientSecret = @"clientsecret", TenantID = @"tentantid", FileName = "/mydata/testfile.txt")]Stream myfile, 
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Example assuming text file 
            using (var reader = new StreamReader(myfile))
            {
                var contents = reader.ReadToEnd();
                log.Info(contents);
                return new OkObjectResult(contents);
            }
            
        }
    }
}
