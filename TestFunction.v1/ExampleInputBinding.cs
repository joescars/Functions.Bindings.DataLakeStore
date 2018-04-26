using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Functions.Bindings.DataLakeStore;

namespace TestFunction.v1
{
    public static class ExampleInputBinding
    {
        [FunctionName("ExampleInputBinding")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            [DataLakeStore(AccountFQDN = @"fqdn", ApplicationId = @"applicationid", ClientSecret = @"clientsecret", TenantID = @"tentantid", FileName = "/mydata/testfile.txt")]Stream myfile,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Example assuming text file 
            using (var reader = new StreamReader(myfile))
            {
                var contents = await reader.ReadToEndAsync();
                log.Info(contents);
                return req.CreateResponse(HttpStatusCode.OK,contents);
            }

        }
    }
}
