using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Configuration;
using System.Net.Http.Headers;

namespace Vsts2git
{
    public static class PublishBuildResult
    {
        class Commiter
        {
            public string name { get; set; }
            public string email { get; set; }
        }

        class BuildContent
        {
            [JsonIgnore]
            public string name { get; set; }
            [JsonIgnore]
            public string result { get; set; }
            [JsonIgnore]
            public string plainContent { get; set; }

            public string message { get; set; }
            public Commiter commiter { get; set; }
            public string content { get; set; }
            public string sha { get; set; }
            public string branch { get; set; }
        }

        class Issue
        {
            public string title { get; set; }
            public string body { get; set; }
            public string[] assignees { get; set; }
            public string[] labels { get; set; }
        }

        class Result
        {
            public string url { get; set; }
            public string issueUrl { get; set; }
        }

        [FunctionName("PublishBuildResult")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req,
            Binder binder,
            TraceWriter log)
        {

            try
            {
                string data = await req.Content.ReadAsStringAsync();
                dynamic buildInfo = JsonConvert.DeserializeObject(data);
                log.Info($"Processing {buildInfo?.message?.text}");

                var queryParams = req.RequestUri.ParseQueryString();
                var repo = queryParams["repo"];
                var owner = queryParams["owner"];
                bool.TryParse(queryParams["createIssue"], out var createIssue);

                BuildContent content = await SetupBuildContent(buildInfo, repo, owner, binder);
                log.Info($"Content ready for build {buildInfo?.resource?.buildNumber}");

                Result result = new Result();
                result.url = await CreateContent(content, repo, owner);
                log.Info($"Content created for build with id {buildInfo?.resource?.id}");

                if (createIssue && (content.result == "failed" || content.result == "partiallySucceded"))
                {
                    Issue issue = SetupIssue(buildInfo?.message?.text.ToString(), content);
                    result.issueUrl = await CreateIssue(issue, repo, owner);
                }

                log.Info($"Page url: {result.url}");
                log.Info($"Issue url: {result.issueUrl}");

                return req.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error.", ex);
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private static async Task<BuildContent> SetupBuildContent(dynamic buildInfo, string repo, string owner, Binder binder)
        {
            string name = $"{buildInfo?.resource?.definition?.id}.md";
            string result = buildInfo?.resource?.result;
            string branch = ConfigurationManager.AppSettings["ContentBranch"];
    
            string logsUrl = await Vsts2git.BuildContent.CopyLogsToBlob(buildInfo, binder);
            StringBuilder contentBuilder = Vsts2git.BuildContent.GetBuilderWithSummary(buildInfo);
            contentBuilder.AppendLine($"Find detailed information in the [build log files]({logsUrl})");

            string plainContent = contentBuilder.ToString();
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainContent));

            BuildContent content = new BuildContent()
            {
                name = name,
                result = result,
                plainContent = plainContent,

                commiter = new Commiter()
                {
                    name = "wasteam",
                    email = "wasteam@outlook.com"
                },
                message = "Vsts build completed",
                content = base64,
                sha = await GetContentSha(name, repo, owner),
                branch = branch
            };
            return content;
        }

        private static async Task<string> GetContentSha(string buildName, string repo, string owner)
        {
            var buildPath = $"vsts-builds/{buildName}";
            string pat = ConfigurationManager.AppSettings["GithubPAT"];
            string url = ConfigurationManager.AppSettings["ContentUrl"]?
                .Replace("%%ACCESS_TOKEN%%", pat)
                .Replace("%%REPO%%", repo)
                .Replace("%%OWNER%%", owner)
                .Replace("%%PATH%%", buildPath);
                
            url = url + $"&ref={ConfigurationManager.AppSettings["ContentBranch"]}";

            string sha = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("vsts2git")));
                var resp = await client.GetAsync(url);
                if(resp.StatusCode == HttpStatusCode.OK)
                {
                    string data = await resp.Content.ReadAsStringAsync();
                    dynamic respContent = JsonConvert.DeserializeObject(data);
                    sha = respContent?.sha;
                }

                return sha;
            }
        }

        private static async Task<string> CreateContent(BuildContent content, string repo, string owner)
        {
            var buildPath = $"vsts-builds/{content.name}";
            string pat = ConfigurationManager.AppSettings["GithubPAT"];
            string url = ConfigurationManager.AppSettings["ContentUrl"].Replace("%%ACCESS_TOKEN%%", pat).Replace("%%REPO%%", repo).Replace("%%OWNER%%", owner).Replace("%%PATH%%", buildPath);

            string sha = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("vsts2git")));
                HttpResponseMessage resp = null;
                resp = await client.PutAsJsonAsync(url, content);


                resp.EnsureSuccessStatusCode();

                string data = await resp.Content.ReadAsStringAsync();
                dynamic respContent = JsonConvert.DeserializeObject(data);
                
                return respContent?.content?.html_url;
            }
        }

        private static Issue SetupIssue(string title, BuildContent buildContent)
        {
            Issue
                issue = new Issue()
                {
                    title = title,
                    body = buildContent.plainContent,
                    labels = buildContent.result == "failed" || buildContent.result == "partiallySucceeded" ? new string[] { "bug", "vsts-build" } : new string[] { "vsts-build" },
                    assignees = new string[0]
                };

            return issue;
        }

        private static async Task<string> CreateIssue(Issue issue, string repo, string owner)
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
    }
}

