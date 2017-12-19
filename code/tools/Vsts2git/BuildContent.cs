using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Vsts2git
{
    public static class BuildContent
    {
        public static async Task<string> CopyLogsToBlob(dynamic buildInfo, Binder binder)
        {
            string pat = ConfigurationManager.AppSettings["VsPAT"];
            string url = buildInfo?.resource?.logs?.url;
            string res = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat))));

                    using (HttpResponseMessage response = client.GetAsync(url + "?$format=zip").Result)
                    {
                        string fileName = buildInfo?.resource?.buildNumber + "_logs.zip";

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            res = await UploadContentToBlob(response.Content, fileName, binder);
                        }
                    }
                }
            }

            return res;
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
                await content.CopyToAsync(stream).ContinueWith(copyTask => { stream.Close(); });
            }
            return ConfigurationManager.AppSettings["DiagBlobUrl"] + "/" + blobPath;
        }
    }
}
