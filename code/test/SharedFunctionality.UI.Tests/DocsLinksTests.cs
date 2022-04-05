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
using Xunit;

namespace Microsoft.Templates.Test.Documentation
{
    // There's a good argument that these tests don't really belong in the UI.Test project
    // However, there is no single good place for them and it felt unnecessary to create a
    // project just for them based on current needs.
    [Trait("Group", "ManualOnly")]
    public class DocsLinksTests : BaseLinkTestLogic
    {
        // This a relative path from where the test assembly will run
        private const string DocsRoot = "../../../../../docs";

        [Fact]
        public async Task DocsLinksAreCorrectAsync()
        {
            var filesWithBrokenLinks = new List<(string file, string uri, int statusCode)>();

            // Check all variations of the description files as localized descriptions may point to localized docs
            foreach (var file in GetFiles(DocsRoot, "*.md"))
            {
                var fileContents = File.ReadAllText(file);
                var links = Regex.Matches(fileContents, pattern, RegexOptions.IgnorePatternWhitespace);

                var httpRoot = "https://github.com/microsoft/TemplateStudio/tree/main/docs";

                foreach (Match link in links)
                {
                    var url = link.Groups[2].Value;

                    var localFilePath = file.Substring(DocsRoot.Length).Replace('\\', '/');

                    if (url.StartsWith("_", StringComparison.Ordinal))
                    {
                        // Some VB code samples can match the RegEx pattern but aren't URLs.
                        continue;
                    }

                    // Note. New files only added locally may cause false negatives
                    if (url.StartsWith("/samples/", StringComparison.Ordinal))
                    {
                        localFilePath = localFilePath.Substring(0, localFilePath.LastIndexOf('/'));
                        url = httpRoot.Substring(0, httpRoot.LastIndexOf('/')) + localFilePath.Substring(0, localFilePath.LastIndexOf('/')) + url;
                    }
                    else if (url.StartsWith("../", StringComparison.Ordinal))
                    {
                        localFilePath = localFilePath.Substring(0, localFilePath.LastIndexOf('/'));
                        url = httpRoot + localFilePath.Substring(0, localFilePath.LastIndexOf('/')) + url.Substring(2);
                    }
                    else if (url.StartsWith("./", StringComparison.Ordinal))
                    {
                        url = httpRoot + localFilePath.Substring(0, localFilePath.LastIndexOf('/')) + url.Substring(1);
                    }
                    else if (url.StartsWith("/", StringComparison.Ordinal))
                    {
                        url = httpRoot + url;
                    }
                    else if (url.StartsWith("#", StringComparison.Ordinal))
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

            foreach ((string file, string uri, int statusCode) in filesWithBrokenLinks)
            {
                errorMessage.AppendLine($"Got a {statusCode} when requesting '{uri}' for '{file}'");
            }

            Assert.True(!filesWithBrokenLinks.Any(), errorMessage.ToString());
        }
    }
}
