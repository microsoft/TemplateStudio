using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft;

namespace Vsts2git
{
    public static class CreateIssueOnBuildFailure
    {
        [FunctionName("CreateIssueOnBuildFailure")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            dynamic buildFailure = await req.Content.ReadAsAsync<object>();

            log.Info(await req.Content.ReadAsStringAsync());

            return req.CreateResponse(HttpStatusCode.OK, "Issue created!");
        }
    }
}
