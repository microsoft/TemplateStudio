// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.Templates.Test
{
    // These tests are all manual as they require network connection to external sites
    // Any failures need to be manually verified and so this should not force the build to fail.
    // We don't want failing build because an external site is temporarily inaccessible
    [Trait("Type", "CodeStyle")]
    [Trait("ExecutionSet", "ManualOnly")]
    public class ExternalLinksTests
    {
        // These are relative paths from where the test assembly will run
        private const string TemplatesRoot = "../../../../../Templates";
        private const string DocsRoot = "../../../../../docs";

        private static List<string> knownGoodUrls = new List<string>();

        [Fact]
        public async Task LicenseLinksAreCorrectAsync()
        {
            var filesWithBrokenLinks = new List<(string file, string uri, int statusCode)>();

            foreach (var file in GetFiles(TemplatesRoot, "template.json"))
            {
                var fileContents = File.ReadAllText(file);
                var info = JsonConvert.DeserializeObject<TemplateValidator.ValidationTemplateInfo>(fileContents);

                var licenses = GetLicenses(info).ToList();

                foreach (var license in licenses)
                {
                    var statusCode = await GetStatusCodeAsync(license.Url);

                    switch (statusCode)
                    {
                        case HttpStatusCode.OK:
                            break;
                        default:
                            filesWithBrokenLinks.Add((file, license.Url, (int)statusCode));
                            break;
                    }
                }
            }

            var errorMessage = new System.Text.StringBuilder();
            errorMessage.AppendLine("Some License links failed:");

            foreach (var brokenLink in filesWithBrokenLinks)
            {
                errorMessage.AppendLine($"Got a {brokenLink.statusCode} when requesting '{brokenLink.uri}' for '{brokenLink.file}'");
            }

            Assert.True(!filesWithBrokenLinks.Any(), errorMessage.ToString());
        }

        private string pattern = @"\[     # Match [
    (        # Match and capture in group 1:
     [^][]*  #  Any number of characters except brackets
    )        # End of capturing group 1
    \]       # Match ]
    \(       # Match (
    (        # Match and capture in group 2:
     [^()]*  #  Any number of characters except parentheses
    )        # End of capturing group 2
    \)       # Match )";

        [Fact]
        public async Task DescriptionLinksAreCorrectAsync()
        {
            var filesWithBrokenLinks = new List<(string file, string uri, int statusCode)>();

            // Check all variations of the description files as localized descriptions may point to localized docs
            foreach (var file in GetFiles(TemplatesRoot, "*description.md"))
            {
                var fileContents = File.ReadAllText(file);
                var links = Regex.Matches(fileContents, pattern, RegexOptions.IgnorePatternWhitespace);

                foreach (Match link in links)
                {
                    var url = link.Groups[2].Value;
                    var statusCode = await GetStatusCodeAsync(url);

                    switch (statusCode)
                    {
                        case HttpStatusCode.OK:
                            break;
                        default:
                            filesWithBrokenLinks.Add((file, url, (int)statusCode));
                            break;
                    }
                }
            }

            var errorMessage = new System.Text.StringBuilder();
            errorMessage.AppendLine("Some Description links failed:");

            foreach (var brokenLink in filesWithBrokenLinks)
            {
                errorMessage.AppendLine($"Got a {brokenLink.statusCode} when requesting '{brokenLink.uri}' for '{brokenLink.file}'");
            }

            Assert.True(!filesWithBrokenLinks.Any(), errorMessage.ToString());
        }

        [Fact]
        public async Task DocsLinksAreCorrectAsync()
        {
            var filesWithBrokenLinks = new List<(string file, string uri, int statusCode)>();

            // Check all variations of the description files as localized descriptions may point to localized docs
            foreach (var file in GetFiles(DocsRoot, "*.md"))
            {
                var fileContents = File.ReadAllText(file);
                var links = Regex.Matches(fileContents, pattern, RegexOptions.IgnorePatternWhitespace);

                var httpRoot = "https://github.com/Microsoft/WindowsTemplateStudio/tree/dev/docs";

                foreach (Match link in links)
                {
                    var url = link.Groups[2].Value;

                    var localFilePath = file.Substring(DocsRoot.Length).Replace('\\', '/');

                    // Note. New files only added locally may cause false negatives
                    if (url.StartsWith("/samples/"))
                    {
                        localFilePath = localFilePath.Substring(0, localFilePath.LastIndexOf('/'));
                        url = httpRoot.Substring(0, httpRoot.LastIndexOf('/')) + localFilePath.Substring(0, localFilePath.LastIndexOf('/')) + url;
                    }
                    else if (url.StartsWith("../"))
                    {
                        localFilePath = localFilePath.Substring(0, localFilePath.LastIndexOf('/'));
                        url = httpRoot + localFilePath.Substring(0, localFilePath.LastIndexOf('/')) + url.Substring(2);
                    }
                    else if (url.StartsWith("./"))
                    {
                        url = httpRoot + localFilePath.Substring(0, localFilePath.LastIndexOf('/')) + url.Substring(1);
                    }
                    else if (url.StartsWith("/"))
                    {
                        url = httpRoot + url;
                    }
                    else if (url.StartsWith("#"))
                    {
                        url = httpRoot + localFilePath + url;
                    }
                    else if (url == "_isAutomaticErrorReportingEnabled, value" || url == "Of T" || url == "_selected, value")
                    {
                        // Above is some code that is incorrectly picked up by the RegEx
                        continue;
                    }

                    var statusCode = await GetStatusCodeAsync(url);

                    switch (statusCode)
                    {
                        case HttpStatusCode.OK:
                            break;
                        default:
                            filesWithBrokenLinks.Add((file, url, (int)statusCode));
                            break;
                    }
                }
            }

            var errorMessage = new System.Text.StringBuilder();
            errorMessage.AppendLine("Some Docs links failed:");

            foreach (var brokenLink in filesWithBrokenLinks)
            {
                errorMessage.AppendLine($"Got a {brokenLink.statusCode} when requesting '{brokenLink.uri}' for '{brokenLink.file}'");
            }

            Assert.True(!filesWithBrokenLinks.Any(), errorMessage.ToString());
        }

        [Fact]
        public async Task CommentLinksAreCorrectAsync()
        {
            var filesWithBrokenLinks = new List<(string file, string uri, int statusCode)>();

            // Just check C# files. A separate test verifies that VB & C# comments are the same.
            foreach (var file in GetFiles(TemplatesRoot, "*.cs"))
            {
                var fileContents = File.ReadAllLines(file);

                var links = new List<string>();

                foreach (var line in fileContents)
                {
                    if (line.TrimStart().StartsWith("// ") && line.Contains("http"))
                    {
                        var httpPos = line.IndexOf("http");

                        var nextSpacePos = line.IndexOf(' ', httpPos);

                        if (nextSpacePos > 0)
                        {
                            links.Add(line.Substring(httpPos, nextSpacePos - httpPos));
                        }
                        else
                        {
                            links.Add(line.Substring(httpPos));
                        }
                    }
                }

                foreach (var url in links)
                {
                    if (url == "https://YourPrivacyUrlGoesHere")
                    {
                        continue;
                    }

                    var statusCode = await GetStatusCodeAsync(url);

                    switch (statusCode)
                    {
                        case HttpStatusCode.OK:
                            break;
                        default:
                            filesWithBrokenLinks.Add((file, url, (int)statusCode));
                            break;
                    }
                }
            }

            var errorMessage = new System.Text.StringBuilder();
            errorMessage.AppendLine("Some comment links failed:");

            foreach (var brokenLink in filesWithBrokenLinks)
            {
                errorMessage.AppendLine($"Got a {brokenLink.statusCode} when requesting '{brokenLink.uri}' for '{brokenLink.file}'");
            }

            Assert.True(!filesWithBrokenLinks.Any(), errorMessage.ToString());
        }

        public static IEnumerable<TemplateLicense> GetLicenses(TemplateValidator.ValidationTemplateInfo ti)
        {
            var licenses = GetValueFromTemplateTag(ti, "wts.licenses");

            if (string.IsNullOrWhiteSpace(licenses))
            {
                return Enumerable.Empty<TemplateLicense>();
            }

            var result = new List<TemplateLicense>();
            var licensesMatches = Regex.Matches(licenses, @"\[(?<text>.*?)\]\((?<url>.*?)\)\|?");

            for (int i = 0; i < licensesMatches.Count; i++)
            {
                var m = licensesMatches[i];

                if (m.Success)
                {
                    result.Add(new TemplateLicense
                    {
                        Text = m.Groups["text"].Value,
                        Url = m.Groups["url"].Value
                    });
                }
            }

            return result;
        }

        private static string GetValueFromTemplateTag(TemplateValidator.ValidationTemplateInfo templateInfo, string tagName)
        {
            if (templateInfo.TemplateTags != null && !string.IsNullOrEmpty(tagName) && templateInfo.TemplateTags.TryGetValue(tagName, out string tagValue))
            {
                return tagValue;
            }

            return null;
        }

        private async Task<HttpStatusCode> GetStatusCodeAsync(string url)
        {
            try
            {
                if (knownGoodUrls.Contains(url))
                {
                    return HttpStatusCode.OK;
                }

                System.Diagnostics.Debug.WriteLine(url);

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

        private IEnumerable<string> GetFiles(string directory, string searchPattern)
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
