using Functions.Bindings.DataLakeStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.IO;

namespace TestFunction.v2
{
    public static class ExampleOutputFromBlob
    {
        [FunctionName("ExampleOutputFromBlob")]
        public static void Run([BlobTrigger("stuff/{name}", Connection = "blobconn")]Stream myBlob, string name,
            [DataLakeStore(AccountFQDN = @"fqdn", ApplicationId = @"applicationid", ClientSecret = @"clientsecret", TenantID = @"tentantid")]out DataLakeStoreOutput dataLakeStoreOutput,
            TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var d = new DataLakeStoreOutput();
            d.filename = "/mydata/" + name;
            d.stream = myBlob;
            dataLakeStoreOutput = d;

        }
    }
}
