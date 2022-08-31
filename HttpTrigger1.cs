using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Company.Function
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            log.LogInformation("Using MessageTemplate with separated parameters {ApplicationName} {FieldValue1}","functionToAppInsights","Value01");

            var customDimensions = new Dictionary<string,object>()
            {
                {"EventCode","DANIEL_HOMEWORK_APPLICATIONPROCESSED"},
                {"EventMessage","There has been some fun here"}                
            };
            using (log.BeginScope(customDimensions))
            {
                log.LogInformation("{EventCode} - Using MessageTemplate with properties object: {EventMessage}");
            }
            
            log.LogInformation("Message using the @ character {@customDimensions}",customDimensions);
            log.LogInformation("Message using customDimensions object", customDimensions);
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
