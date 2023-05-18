using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Contoso
{
    public static class ProcessSoapMsg
    {
        [FunctionName("ProcessSoapMsg")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {        
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            string responseMessage = $"Here the SOAP request {requestBody}";

            return new OkObjectResult(responseMessage);
        }
    }
}
