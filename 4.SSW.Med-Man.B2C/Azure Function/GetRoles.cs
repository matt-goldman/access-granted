using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class GetRoles
    {
        [FunctionName("GetRoles")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            log.LogInformation("Request body:");
            log.LogInformation(requestBody);

            string email = data?.email;

            string title = data?.jobTitle;

            return (ActionResult)new OkObjectResult(
                    new ResponseContent
                    {
                        version = "1.1.0",
                        status = (int) HttpStatusCode.OK,
                        role = GetRole(title)
                    }
                );
        }
        public static string GetRole(string emailPart)
        {
            switch (emailPart.ToLower())
            {
                case "doctor":
                    return "Doctor";
                case "nurse":
                    return "Nurse";
                case "pharmacist":
                    return "Pharmacist";
                default:
                    return "Patient";
            }
        }
    }

    public class ResponseContent
    {
        public string version { get; set; }
        public int status { get; set; }
        public string role { get; set; }
    }
}
