// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Microsoft.Templates.Test
{
    public class BaseLinkTestLogic
    {
        protected string pattern = @"\[     # Match [
    (        # Match and capture in group 1:
     [^][]*  #  Any number of characters except brackets
    )        # End of capturing group 1
    \]       # Match ]
    \(       # Match (
    (        # Match and capture in group 2:
     [^()]*  #  Any number of characters except parentheses
    )        # End of capturing group 2
    \)       # Match )";

        private static List<string> knownGoodUrls = new List<string>();

        protected async Task<HttpStatusCode> GetStatusCodeAsync(string url)
        {
            try
            {
                if (url.EndsWith(".", StringComparison.Ordinal))
                {
                    url = url.TrimEnd('.');
                }

                if (knownGoodUrls.Contains(url))
                {
                    return HttpStatusCode.OK;
                }

                System.Diagnostics.Debug.WriteLine(url);

                // No longer needed...for some reason 🤷
                // Ensure using strong SSL certificates (Necessary for github URLs)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };

                var req = new HttpClient();
                var resp = await req.GetAsync(url);

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    if (!knownGoodUrls.Contains(url))
                    {
                        knownGoodUrls.Add(url);
                    }
                }

                return resp.StatusCode;
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
                return HttpStatusCode.BadGateway; // Generic error (502)
            }
        }

        protected IEnumerable<string> GetFiles(string directory, string searchPattern)
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                foreach (var file in Directory.GetFiles(dir, searchPattern))
                {
                    yield return file;
                }

                foreach (var file in GetFiles(dir, searchPattern))
                {
                    yield return file;
                }
            }
        }
    }
}
