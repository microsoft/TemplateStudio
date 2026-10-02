// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Templates.Test;
using Vsts2git;
using Xunit;

namespace Microsoft.Templates.Security.Test
{
    [CollectionDefinition("Network security", DisableParallelization = true)]
    public sealed class NetworkSecurityCollection
    {
    }

    [Collection("Network security")]
    [Trait("Group", "Minimum")]
    public class NetworkSecurityTests
    {
        [Fact]
        public async Task DefaultLogClientDoesNotFollowRedirects()
        {
            var destination = new LoopbackServer();
            var origin = new LoopbackServer(HttpStatusCode.Redirect, destination.Url);
            try
            {
                using (HttpClient client = BuildContent.CreateLogDownloadClient())
                using (HttpResponseMessage response = await client.GetAsync(origin.Url))
                {
                    Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
                    Assert.Equal(destination.Url, response.Headers.Location);
                    Assert.False(destination.RequestAccepted);
                }

                ServerResult result = await origin.Completion;
                Assert.Null(result.Error);
                Assert.False(result.TimedOut);
                Assert.NotNull(result.RequestHeaders);
            }
            finally
            {
                await origin.StopAsync();
                await destination.StopAsync();
            }
        }

        [Fact]
        public async Task LinkRequestsPreserveTlsPolicyAndSuccessfulUrlCaching()
        {
            RemoteCertificateValidationCallback callback = ServicePointManager.ServerCertificateValidationCallback;
            SecurityProtocolType protocols = ServicePointManager.SecurityProtocol;
            var server = new LoopbackServer();
            try
            {
                var links = new LinkProbe();
                Assert.Equal(HttpStatusCode.OK, await links.GetStatus(server.Url.AbsoluteUri + "."));
                Assert.Same(callback, ServicePointManager.ServerCertificateValidationCallback);
                Assert.Equal(protocols, ServicePointManager.SecurityProtocol);

                ServerResult result = await server.Completion;
                Assert.Null(result.Error);
                Assert.False(result.TimedOut);
                Assert.NotNull(result.RequestHeaders);

                await server.StopAsync();
                Assert.Equal(HttpStatusCode.OK, await links.GetStatus(server.Url.AbsoluteUri));
            }
            finally
            {
                // Restore the captured policy when exercising the unfixed helper as a regression control.
                ServicePointManager.ServerCertificateValidationCallback = callback;
                ServicePointManager.SecurityProtocol = protocols;
                await server.StopAsync();
            }
        }

        [Fact]
        public async Task LinkRequestsRejectUntrustedHttpsCertificates()
        {
            RemoteCertificateValidationCallback callback = ServicePointManager.ServerCertificateValidationCallback;
            SecurityProtocolType protocols = ServicePointManager.SecurityProtocol;
            Assert.Null(callback);

            using (X509Certificate2 certificate = CreateTestCertificate())
            {
                // A pinned, loopback-only control proves the TLS fixture can complete a real request.
                var control = new LoopbackServer(certificate: certificate);
                try
                {
                    using (var socket = new TcpClient())
                    {
                        await socket.ConnectAsync(IPAddress.Loopback, control.Url.Port);
                        using (var tls = new SslStream(socket.GetStream(), false, (sender, remote, chain, errors) =>
                            remote != null && remote.GetCertHashString() == certificate.GetCertHashString()))
                        {
                            await tls.AuthenticateAsClientAsync("localhost", null, SslProtocols.Tls12, false);
                            byte[] request = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: localhost\r\nConnection: close\r\n\r\n");
                            await tls.WriteAsync(request, 0, request.Length);
                            using (var reader = new StreamReader(tls, Encoding.ASCII, false, 1024, true))
                            {
                                Assert.StartsWith("HTTP/1.1 200", await reader.ReadLineAsync());
                            }
                        }
                    }

                    ServerResult controlResult = await control.Completion;
                    Assert.Null(controlResult.Error);
                    Assert.False(controlResult.TimedOut);
                    Assert.NotNull(controlResult.RequestHeaders);
                }
                finally
                {
                    await control.StopAsync();
                }

                var server = new LoopbackServer(certificate: certificate);
                try
                {
                    HttpStatusCode result = await new LinkProbe().GetStatus(server.Url.AbsoluteUri);
                    Assert.Equal(HttpStatusCode.BadGateway, result);
                    Assert.Same(callback, ServicePointManager.ServerCertificateValidationCallback);
                    Assert.Equal(protocols, ServicePointManager.SecurityProtocol);

                    ServerResult serverResult = await server.Completion;
                    Assert.True(server.RequestAccepted);
                    Assert.False(serverResult.TimedOut);
                    Assert.Null(serverResult.RequestHeaders);
                    Assert.NotNull(serverResult.Error);
                }
                finally
                {
                    ServicePointManager.ServerCertificateValidationCallback = callback;
                    ServicePointManager.SecurityProtocol = protocols;
                    await server.StopAsync();
                }
            }
        }

        private static X509Certificate2 CreateTestCertificate()
        {
            using (RSA key = RSA.Create())
            {
                key.KeySize = 2048;
                var request = new CertificateRequest("CN=localhost", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                using (X509Certificate2 certificate = request.CreateSelfSigned(
                    DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(5)))
                {
                    return new X509Certificate2(certificate.Export(X509ContentType.Pfx));
                }
            }
        }

        private sealed class LinkProbe : BaseLinkTestLogic
        {
            public Task<HttpStatusCode> GetStatus(string url) => GetStatusCodeAsync(url);
        }

        private sealed class ServerResult
        {
            public string RequestHeaders { get; set; }

            public Exception Error { get; set; }

            public bool TimedOut { get; set; }
        }

        private sealed class LoopbackServer
        {
            private readonly TcpListener _listener = new TcpListener(IPAddress.Loopback, 0);
            private readonly CancellationTokenSource _deadline = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            private readonly CancellationTokenRegistration _stopListener;
            private readonly HttpStatusCode _statusCode;
            private readonly Uri _location;
            private readonly X509Certificate2 _certificate;
            private bool _stopped;

            public LoopbackServer(HttpStatusCode statusCode = HttpStatusCode.OK, Uri location = null, X509Certificate2 certificate = null)
            {
                _statusCode = statusCode;
                _location = location;
                _certificate = certificate;
                _listener.Start();
                _stopListener = _deadline.Token.Register(_listener.Stop);
                int port = ((IPEndPoint)_listener.LocalEndpoint).Port;
                Url = new Uri((certificate == null ? "http" : "https") + "://127.0.0.1:" + port + "/" + Guid.NewGuid().ToString("N"));
                Completion = RespondAsync();
            }

            public Uri Url { get; }

            public Task<ServerResult> Completion { get; }

            public bool RequestAccepted { get; private set; }

            public async Task StopAsync()
            {
                if (_stopped)
                {
                    return;
                }

                _stopped = true;
                _deadline.Cancel();
                await Completion;
                _stopListener.Dispose();
                _deadline.Dispose();
            }

            private async Task<ServerResult> RespondAsync()
            {
                try
                {
                    using (TcpClient socket = await _listener.AcceptTcpClientAsync())
                    using (_deadline.Token.Register(socket.Close))
                    {
                        RequestAccepted = true;
                        if (_certificate != null)
                        {
                            using (var tls = new SslStream(socket.GetStream(), false))
                            {
                                await tls.AuthenticateAsServerAsync(_certificate, false, SslProtocols.Tls12, false);
                                return await RespondAsync(tls);
                            }
                        }

                        return await RespondAsync(socket.GetStream());
                    }
                }
                catch (AuthenticationException exception)
                {
                    return Failure(exception);
                }
                catch (IOException exception)
                {
                    return Failure(exception);
                }
                catch (SocketException exception) when (_deadline.IsCancellationRequested)
                {
                    return Failure(exception);
                }
                catch (ObjectDisposedException exception) when (_deadline.IsCancellationRequested)
                {
                    return Failure(exception);
                }
            }

            private async Task<ServerResult> RespondAsync(Stream stream)
            {
                var headers = new StringBuilder();
                using (var reader = new StreamReader(stream, Encoding.ASCII, false, 1024, true))
                {
                    while (true)
                    {
                        string line = await reader.ReadLineAsync();
                        if (line == null)
                        {
                            throw new IOException("Connection closed before request headers.");
                        }

                        headers.AppendLine(line);
                        if (headers.Length > 8192)
                        {
                            throw new InvalidDataException("Test request headers exceeded their limit.");
                        }

                        if (line.Length == 0)
                        {
                            break;
                        }
                    }
                }

                string response = "HTTP/1.1 " + (int)_statusCode + " Test\r\nContent-Length: 2\r\nConnection: close\r\n";
                if (_location != null)
                {
                    response += "Location: " + _location.AbsoluteUri + "\r\n";
                }

                byte[] bytes = Encoding.ASCII.GetBytes(response + "\r\nok");
                await stream.WriteAsync(bytes, 0, bytes.Length);
                return new ServerResult { RequestHeaders = headers.ToString() };
            }

            private ServerResult Failure(Exception exception) => new ServerResult
            {
                Error = exception,
                TimedOut = _deadline.IsCancellationRequested && !_stopped,
            };
        }
    }
}
