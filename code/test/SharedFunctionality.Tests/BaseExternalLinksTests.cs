// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    public abstract class BaseExternalLinksTests : BaseLinkTestLogic
    {
        public abstract Task LicenseLinksAreCorrectAsync();

        public abstract Task DescriptionLinksAreCorrectAsync();

        public abstract Task CommentLinksAreCorrectAsync();

        public async Task CheckLicenseLinksInternalAsync(string relativeTemplateRoot)
        {
            var errorMessage = new System.Text.StringBuilder();
            var filesWithBrokenLinks = new List<(string file, string uri, int statusCode)>();

            foreach (var file in GetFiles(relativeTemplateRoot, "template.json"))
            {
                try
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
                catch (Exception e)
                {
                    errorMessage.AppendLine($"Error with '{file}'");
                    errorMessage.AppendLine(e.Message);
                }
            }

            if (filesWithBrokenLinks.Any())
            {
                errorMessage.AppendLine("Some License links failed:");

                foreach (var brokenLink in filesWithBrokenLinks)
                {
                    errorMessage.AppendLine(
                        $"Got a {brokenLink.statusCode} when requesting '{brokenLink.uri}' for '{brokenLink.file}'");
                }
            }

            Assert.True(errorMessage.Length == 0, errorMessage.ToString());
        }

        public async Task DescriptionLinkTestsInternalAsync(string relativeTemplateRoot)
        {
            var filesWithBrokenLinks = new List<(string file, string uri, int statusCode)>();

            // Check all variations of the description files as localized descriptions may point to localized docs
            foreach (var file in GetFiles(relativeTemplateRoot, "*description.md"))
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

        public async Task CommentLinkTestsInternalAsync(string relativeTemplateRoot)
        {
            var filesWithBrokenLinks = new List<(string file, string uri, int statusCode)>();
            var insideCommentBlock = false;

            // Just check C# files. A separate test verifies that VB & C# comments are the same.
            foreach (var file in GetFiles(relativeTemplateRoot, "*.cs"))
            {
                var fileContents = File.ReadAllLines(file);

                var links = new List<string>();

                foreach (var line in fileContents)
                {
                    if (line.TrimStart().StartsWith("/*", StringComparison.Ordinal))
                    {
                        insideCommentBlock = true;
                    }

                    if ((line.TrimStart().StartsWith("// ", StringComparison.Ordinal) || insideCommentBlock) && line.Contains("http"))
                    {
                        var httpPos = line.IndexOf("http", StringComparison.Ordinal);

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

                    if (line.TrimEnd().EndsWith("*/", StringComparison.Ordinal))
                    {
                        insideCommentBlock = false;
                    }
                }

                foreach (var url in links)
                {
                    // The login.microsoftonline address is from the SecureWebAPI code and calls to it may be affected by proxy settings
                    if (url == "https://YourPrivacyUrlGoesHere"
                     || url.StartsWith("https://login.microsoftonline.com/common/discovery/instance?authorization_endpoint=https", StringComparison.Ordinal))
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
                        Url = m.Groups["url"].Value,
                    });
                }
            }

            return result;
        }

        private static string GetValueFromTemplateTag(TemplateValidator.ValidationTemplateInfo templateInfo, string tagName)
        {
            if (templateInfo.TagsCollection != null && !string.IsNullOrEmpty(tagName) && templateInfo.TagsCollection.TryGetValue(tagName, out string tagValue))
            {
                return tagValue;
            }

            return null;
        }
    }
}
