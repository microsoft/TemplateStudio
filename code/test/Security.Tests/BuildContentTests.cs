// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json.Linq;
using Vsts2git;
using Xunit;

namespace Microsoft.Templates.Security.Test
{
    [Trait("Group", "Minimum")]
    public class BuildContentTests
    {
        private const string ProjectUrl = "https://dev.azure.com/example/project";
        private const string TestPat = "not-a-real-pat";

        [Theory]
        [InlineData("https://attacker.invalid/logs")]
        [InlineData("https://dev.azure.com.attacker.invalid/logs")]
        [InlineData("https://dev.azure.com@attacker.invalid/logs")]
        [InlineData("https://dev.azure.com:8443/example/project/logs")]
        [InlineData("http://dev.azure.com/example/project/logs")]
        [InlineData("http://169.254.169.254/metadata/")]
        [InlineData("https://127.0.0.1/logs")]
        [InlineData("file:///C:/logs")]
        [InlineData(null)]
        public async Task CallerLogUrlCannotChooseCredentialDestination(string callerUrl)
        {
            var content = Encoding.ASCII.GetBytes("test log archive");
            var handler = new RecordingHandler(() => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(content),
            });
            var binder = new RecordingBinder();
            JObject buildInfo = CreateBuildInfo("42", callerUrl);
            buildInfo["resource"]["id"] = 42;

            using (HttpClient client = BuildContent.CreateLogDownloadClient(handler))
            {
                string result = await BuildContent.CopyLogsToBlob(
                    buildInfo, binder, client, ProjectUrl, TestPat);

                Assert.Equal(new Uri(ProjectUrl + "/_apis/build/builds/42/logs?api-version=7.1"), handler.RequestUri);
                Assert.Equal("Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(":" + TestPat)), handler.Authorization);
                Assert.Equal("application/zip", handler.Accept);
                Assert.Null(client.DefaultRequestHeaders.Authorization);
                Assert.Equal(1, handler.RequestCount);
                Assert.Equal(1, binder.BindingCount);
                Assert.Equal("buildlogs/test-build_logs.zip", binder.BlobPath);
                Assert.Equal(content, binder.Content.ToArray());
                Assert.EndsWith("/buildlogs/test-build_logs.zip", result);
            }
        }

        [Fact]
        public async Task MissingResourceFailsBeforeNetworkOrStorage()
        {
            var handler = new RecordingHandler(() => new HttpResponseMessage(HttpStatusCode.OK));
            var binder = new RecordingBinder();

            using (HttpClient client = BuildContent.CreateLogDownloadClient(handler))
            {
                await Assert.ThrowsAsync<ArgumentException>(() => BuildContent.CopyLogsToBlob(
                    new JObject(), binder, client, ProjectUrl, TestPat));

                Assert.Equal(0, handler.RequestCount);
                Assert.Equal(0, binder.BindingCount);
            }
        }

        [Theory]
        [InlineData("https://dev.azure.com/example/project", "42", "https://dev.azure.com/example/project/_apis/build/builds/42/logs?api-version=7.1")]
        [InlineData("https://example.visualstudio.com/DefaultCollection/project/", "00042", "https://example.visualstudio.com/DefaultCollection/project/_apis/build/builds/42/logs?api-version=7.1")]
        [InlineData("https://dev.azure.com/example/My%20Project/", "2147483647", "https://dev.azure.com/example/My%20Project/_apis/build/builds/2147483647/logs?api-version=7.1")]
        public void ConfiguredProjectAndValidatedBuildIdFormTheRequest(string projectUrl, string buildId, string expected)
        {
            Assert.Equal(new Uri(expected), BuildContent.GetBuildLogsUri(projectUrl, buildId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("+1")]
        [InlineData(" 42")]
        [InlineData("42 ")]
        [InlineData("1.0")]
        [InlineData("1e2")]
        [InlineData("2147483648")]
        [InlineData("../other-project")]
        [InlineData("42?redirect=https://attacker.invalid")]
        public async Task InvalidBuildIdFailsBeforeNetworkOrStorage(string buildId)
        {
            var handler = new RecordingHandler(() => new HttpResponseMessage(HttpStatusCode.OK));
            var binder = new RecordingBinder();

            using (HttpClient client = BuildContent.CreateLogDownloadClient(handler))
            {
                await Assert.ThrowsAsync<ArgumentException>(() => BuildContent.CopyLogsToBlob(
                    CreateBuildInfo(buildId, "https://attacker.invalid/logs"), binder, client, ProjectUrl, TestPat));

                Assert.Equal(0, handler.RequestCount);
                Assert.Equal(0, binder.BindingCount);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("/example/project")]
        [InlineData("http://dev.azure.com/example/project")]
        [InlineData("https://dev.azure.com:8443/example/project")]
        [InlineData("https://user@dev.azure.com/example/project")]
        [InlineData("https://dev.azure.com/example/project?redirect=other")]
        [InlineData("https://dev.azure.com/example/project#fragment")]
        [InlineData("https://dev.azure.com/")]
        [InlineData("https://127.0.0.1/project")]
        [InlineData("https://[::1]/project")]
        [InlineData("https://localhost/project")]
        public async Task InvalidProjectConfigurationFailsBeforeNetworkOrStorage(string projectUrl)
        {
            var handler = new RecordingHandler(() => new HttpResponseMessage(HttpStatusCode.OK));
            var binder = new RecordingBinder();

            using (HttpClient client = BuildContent.CreateLogDownloadClient(handler))
            {
                await Assert.ThrowsAsync<ConfigurationErrorsException>(() => BuildContent.CopyLogsToBlob(
                    CreateBuildInfo("42", null), binder, client, projectUrl, TestPat));

                Assert.Equal(0, handler.RequestCount);
                Assert.Equal(0, binder.BindingCount);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task MissingPatFailsBeforeNetworkOrStorage(string pat)
        {
            var handler = new RecordingHandler(() => new HttpResponseMessage(HttpStatusCode.OK));
            var binder = new RecordingBinder();

            using (HttpClient client = BuildContent.CreateLogDownloadClient(handler))
            {
                await Assert.ThrowsAsync<ConfigurationErrorsException>(() => BuildContent.CopyLogsToBlob(
                    CreateBuildInfo("42", null), binder, client, ProjectUrl, pat));

                Assert.Equal(0, handler.RequestCount);
                Assert.Equal(0, binder.BindingCount);
            }
        }

        [Theory]
        [InlineData(301)]
        [InlineData(302)]
        [InlineData(307)]
        [InlineData(308)]
        [InlineData(403)]
        [InlineData(404)]
        [InlineData(503)]
        public async Task RedirectsAndErrorsDoNotPublishLogs(int statusCode)
        {
            var handler = new RecordingHandler(() =>
            {
                var response = new HttpResponseMessage((HttpStatusCode)statusCode)
                {
                    Content = new StringContent("not a log archive"),
                };
                response.Headers.Location = new Uri("https://attacker.invalid/logs");
                return response;
            });
            var binder = new RecordingBinder();

            using (HttpClient client = BuildContent.CreateLogDownloadClient(handler))
            {
                await Assert.ThrowsAsync<HttpRequestException>(() => BuildContent.CopyLogsToBlob(
                    CreateBuildInfo("42", null), binder, client, ProjectUrl, TestPat));

                Assert.Equal(1, handler.RequestCount);
                Assert.Equal(0, binder.BindingCount);
            }
        }

        [Fact]
        public void DownloadLimitsMatchTheApprovedValues()
        {
            using (HttpClient client = BuildContent.CreateLogDownloadClient())
            {
                Assert.Equal(104857600L, client.MaxResponseContentBufferSize);
                Assert.Equal(TimeSpan.FromSeconds(12000), client.Timeout);
            }
        }

        [Theory]
        [InlineData(true, 104857600L, true)]
        [InlineData(true, 104857601L, false)]
        [InlineData(false, 104857600L, true)]
        [InlineData(false, 104857601L, false)]
        public async Task SizeLimitCoversDeclaredAndUndeclaredResponseLengths(bool declareLength, long length, bool accepted)
        {
            var handler = new RecordingHandler(() => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new GeneratedContent(length, declareLength),
            });

            using (HttpClient client = BuildContent.CreateLogDownloadClient(handler))
            {
                if (accepted)
                {
                    using (HttpResponseMessage response = await client.GetAsync(ProjectUrl))
                    {
                        Stream buffered = await response.Content.ReadAsStreamAsync();
                        Assert.Equal(length, buffered.Length);
                    }
                }
                else
                {
                    await Assert.ThrowsAsync<HttpRequestException>(() => client.GetAsync(ProjectUrl));
                }
            }
        }

        [Fact]
        public async Task StorageWriteFailureIsNotReportedAsSuccess()
        {
            var handler = new RecordingHandler(() => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("test log archive"),
            });
            var binder = new RecordingBinder(new FailingWriteStream());

            using (HttpClient client = BuildContent.CreateLogDownloadClient(handler))
            {
                HttpRequestException error = await Assert.ThrowsAsync<HttpRequestException>(() => BuildContent.CopyLogsToBlob(
                    CreateBuildInfo("42", null), binder, client, ProjectUrl, TestPat));
                Assert.IsType<IOException>(error.InnerException);
            }
        }

        private static JObject CreateBuildInfo(string buildId, string logsUrl)
        {
            var resource = new JObject
            {
                ["id"] = buildId,
                ["buildNumber"] = "test-build",
            };
            if (logsUrl != null)
            {
                resource["logs"] = new JObject { ["url"] = logsUrl };
            }

            return new JObject { ["resource"] = resource };
        }

        private sealed class RecordingHandler : HttpMessageHandler
        {
            private readonly Func<HttpResponseMessage> _response;

            public RecordingHandler(Func<HttpResponseMessage> response) => _response = response;

            public Uri RequestUri { get; private set; }

            public string Authorization { get; private set; }

            public string Accept { get; private set; }

            public int RequestCount { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestCount++;
                RequestUri = request.RequestUri;
                Authorization = request.Headers.Authorization?.ToString();
                Accept = request.Headers.Accept.ToString();
                return Task.FromResult(_response());
            }
        }

        private sealed class RecordingBinder : IBinder
        {
            public RecordingBinder(MemoryStream content = null) => Content = content ?? new MemoryStream();

            public MemoryStream Content { get; }

            public string BlobPath { get; private set; }

            public int BindingCount { get; private set; }

            public Task<T> BindAsync<T>(Attribute attribute, CancellationToken cancellationToken = default)
            {
                Assert.Equal(typeof(Stream), typeof(T));
                BlobPath = Assert.IsType<BlobAttribute>(attribute).BlobPath;
                BindingCount++;
                return Task.FromResult((T)(object)Content);
            }
        }

        private sealed class GeneratedContent : HttpContent
        {
            private readonly long _length;
            private readonly bool _declareLength;

            public GeneratedContent(long length, bool declareLength)
            {
                _length = length;
                _declareLength = declareLength;
            }

            protected override bool TryComputeLength(out long length)
            {
                length = _length;
                return _declareLength;
            }

            protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                var buffer = new byte[65536];
                long remaining = _length;
                while (remaining > 0)
                {
                    int count = (int)Math.Min(buffer.Length, remaining);
                    await stream.WriteAsync(buffer, 0, count);
                    remaining -= count;
                }
            }
        }

        private sealed class FailingWriteStream : MemoryStream
        {
            public override void Write(byte[] buffer, int offset, int count) => throw new IOException("Test storage failure.");

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
                => Task.FromException(new IOException("Test storage failure."));
        }
    }
}
