using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft;
using System;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.IO;
using System.Text;

namespace Vsts2git
{
    public static class CreateIssueOnBuildFailure
    {
        class GitHubIssue
        {
            public string title { get; set; }
            public string body { get; set; }
            public string[] assignees { get; set; }
            public string[] labels { get; set; }
        }

        class GitHubBlob
        {
            public string content { get; set; }
            public string encoding { get; set; }
        }

        class Result
        {
            public string issueUrl { get; set; }
        }

        [FunctionName("CreateIssueOnBuildFailure")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req,
            Binder binder,
            TraceWriter log)
        {
            try
            {
                string data = await req.Content.ReadAsStringAsync();
                dynamic buildFailure = JsonConvert.DeserializeObject(data);
                log.Info($"Processing {buildFailure.message.text}");

                string blobPath = await CopyLogsToBlob(buildFailure, binder);
                log.Info($"Logs copied to {blobPath}");

                GitHubIssue issue = SetupIssue(buildFailure, blobPath);
                var queryParams = req.RequestUri.ParseQueryString();

                Result res = new Result()
                {
                    issueUrl = await CreateIssue(issue, queryParams["repo"], queryParams["owner"])
                };

                log.Info($"Issue created: {res.issueUrl}");

                return req.CreateResponse(HttpStatusCode.OK, res);
            }
            catch(Exception ex)
            {
                log.Error("Unexpected error.", ex);
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private static async Task<string> CopyLogsToBlob(dynamic buildInfo, Binder binder)
        {
            string pat = ConfigurationManager.AppSettings["VsPAT"];
            string url = buildInfo?.resource?.logs?.url;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat))));

                using (HttpResponseMessage response = client.GetAsync(url + "?$format=zip").Result)
                {
                    string fileName = buildInfo?.resource?.buildNumber + "_logs.zip";
                    response.EnsureSuccessStatusCode();

                    return await UploadContentToBlob(response.Content, fileName, binder);
                }
            }
        }


        private static async Task<string> UploadContentToBlob(HttpContent content, string blobFileName, Binder binder)
        {
            var blobPath = $"buildlogs/{blobFileName}";

            using (var stream = await binder.BindAsync<Stream>(new BlobAttribute(blobPath, FileAccess.Write)))
            {
                await content.CopyToAsync(stream).ContinueWith(copyTask => { stream.Close(); });
            }
            return ConfigurationManager.AppSettings["DiagBlobUrl"] + "/" + blobPath;
        }

        private static GitHubIssue SetupIssue(dynamic buildInfo, string blobUrl)
        {
            StringBuilder bodyBuilder = new StringBuilder();

            bodyBuilder.AppendLine("A long running VSTS build have failed. The link to the VSTS build definition requires VSTS valid account but below you can find detailed information and a link to download the build log files.");
            bodyBuilder.AppendLine();
            string detailedInfo = buildInfo?.detailedMessage?.markdown;
            bodyBuilder.AppendLine(detailedInfo);
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine($"Find detailed information in the [build log files]({blobUrl})");

            GitHubIssue
                issue = new GitHubIssue()
            {
                title = buildInfo.message.text,
                body = bodyBuilder.ToString(),
                labels = buildInfo.resource.result == "failed" || buildInfo.resource.result == "partiallySucceeded" ? new string[] { "bug" } : new string[0],
                assignees = new string[0]
            };

            return issue;
        }

        private static async Task<string> CreateIssue(GitHubIssue issue, string repo, string owner)
        {
            string pat = ConfigurationManager.AppSettings["GithubPAT"];
            string url = ConfigurationManager.AppSettings["CreateIssueUrl"].Replace("%%ACCESS_TOKEN%%", pat).Replace("%%REPO%%", repo).Replace("%%OWNER%%", owner);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("vsts2git")));
                var resp = await client.PostAsJsonAsync(url, issue);
                resp.EnsureSuccessStatusCode();

                string data = await resp.Content.ReadAsStringAsync();
                dynamic respContent = JsonConvert.DeserializeObject(data);

                return respContent?.html_url;
            }
        }

        private static Task ReadAsFileAsync(HttpContent content, string filename, bool overwrite)
        {
            string pathname = Path.GetFullPath(filename);
            if (!overwrite && File.Exists(filename))
            {
                throw new InvalidOperationException(string.Format("File {0} already exists.", pathname));
            }

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(pathname, FileMode.Create, FileAccess.Write, FileShare.None);
                return content.CopyToAsync(fileStream).ContinueWith(
                   (copyTask) =>
                   {
                       fileStream.Close();
                   });
            }
            catch
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }

                throw;
            }
        }
    }
}
