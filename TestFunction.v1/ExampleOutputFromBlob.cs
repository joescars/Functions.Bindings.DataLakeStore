using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Functions.Bindings.DataLakeStore;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace TestFunction.v1
{
    public static class ExampleOutputFromBlob
    {
        [FunctionName("ExampleOutputFromBlob")]
        public static void Run([BlobTrigger("stuff/{name}", Connection = @"blobconn")]Stream myBlob, 
            [DataLakeStore(AccountFQDN = @"fqdn", ApplicationId = @"applicationid", ClientSecret = @"clientsecret", TenantID = @"tentantid")]out DataLakeStoreOutput dataLakeStoreOutput,
            string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var d = new DataLakeStoreOutput();
            d.filename = "/mydata/" + name;
            d.stream = myBlob;            
            dataLakeStoreOutput = d;

        }
    }
}
