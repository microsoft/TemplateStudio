using Microsoft.Azure.WebJobs;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Vsts2git
{
    public static class BuildContent
    {
        private const long MaxLogDownloadBytes = 100L * 1024 * 1024;
        private const int LogDownloadTimeoutSeconds = 12000;

        public static async Task<string> CopyLogsToBlob(dynamic buildInfo, Binder binder)
        {
            string buildId = buildInfo?.resource?.id?.ToString();
            Uri logsUri = GetBuildLogsUri(ConfigurationManager.AppSettings["VsProjectUrl"], buildId);
            string pat = ConfigurationManager.AppSettings["VsPAT"];

            if (string.IsNullOrWhiteSpace(pat))
            {
                throw new ConfigurationErrorsException("VsPAT must be configured for build-log downloads.");
            }

            using (var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false })
            {
                Timeout = TimeSpan.FromSeconds(LogDownloadTimeoutSeconds),
                MaxResponseContentBufferSize = MaxLogDownloadBytes,
            })
            using (var request = new HttpRequestMessage(HttpMethod.Get, logsUri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(":" + pat)));

                // Buffering applies the client's size limit and timeout to the complete download before publishing.
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    string fileName = buildInfo?.resource?.buildNumber + "_logs.zip";
                    return await UploadContentToBlob(response.Content, fileName, binder);
                }
            }
        }

        private static Uri GetBuildLogsUri(string projectUrl, string buildId)
        {
            if (!int.TryParse(buildId, NumberStyles.None, CultureInfo.InvariantCulture, out int id) || id <= 0)
            {
                throw new ArgumentException("The build event must contain a positive integer resource.id.", nameof(buildId));
            }

            if (!Uri.TryCreate(projectUrl, UriKind.Absolute, out Uri projectUri)
                || projectUri.Scheme != Uri.UriSchemeHttps
                || !projectUri.IsDefaultPort
                || projectUri.HostNameType != UriHostNameType.Dns
                || projectUri.IsLoopback
                || !string.IsNullOrEmpty(projectUri.UserInfo)
                || !string.IsNullOrEmpty(projectUri.Query)
                || !string.IsNullOrEmpty(projectUri.Fragment)
                || projectUri.AbsolutePath.TrimEnd('/').Length == 0)
            {
                throw new ConfigurationErrorsException(
                    "VsProjectUrl must be a trusted HTTPS Azure DevOps project URL using the default port, without userinfo, a query, or a fragment.");
            }

            // The webhook's logs.url must never select a destination for the server credential.
            return new Uri(projectUri.AbsoluteUri.TrimEnd('/') + "/_apis/build/builds/"
                + id.ToString(CultureInfo.InvariantCulture) + "/logs?api-version=7.1");
        }

        public static StringBuilder GetBuilderWithSummary(dynamic buildInfo)
        {
            DateTime finish = buildInfo?.resource?.finishTime;
            DateTime start = buildInfo?.resource?.startTime;
            TimeSpan duration = new TimeSpan((finish - start).Ticks);
            DateTime queued = buildInfo?.resource?.queueTime;

            StringBuilder contentBuilder = new StringBuilder($"## Build {buildInfo?.resource?.buildNumber}\r\n");
            contentBuilder.AppendLine($"- **Build result:** `{buildInfo?.resource?.result.ToString()}`");
            contentBuilder.AppendLine($"- **Build queued:** {queued.ToString()}");
            contentBuilder.AppendLine($"- **Build duration:** {duration.TotalMinutes:0.00} minutes");

            contentBuilder.AppendLine($"### Details");
            contentBuilder.AppendLine(buildInfo?.detailedMessage?.markdown.ToString());
            return contentBuilder;
        }


        private static async Task<string> UploadContentToBlob(HttpContent content, string blobFileName, Binder binder)
        {
            var blobPath = $"buildlogs/{blobFileName}";

            using (var stream = await binder.BindAsync<Stream>(new BlobAttribute(blobPath, FileAccess.Write)))
            {
                await content.CopyToAsync(stream);
            }
            return ConfigurationManager.AppSettings["DiagBlobUrl"] + "/" + blobPath;
        }
    }
}
