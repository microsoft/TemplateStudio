// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Test
{
    public sealed class LegacyTemplatesSource : TemplatesSource
    {
        private static string TemplatesVersion { get; } = "1.5.17334.07";
        private string TemplatesPackageFileName { get; } = $"pro.Templates_{TemplatesVersion}.mstx";

        private readonly string _cdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

        public override bool ForcedAcquisition => true;

        public override string Id => "TestLegacy" + GetAgentName();

        protected override string AcquireMstx()
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            var sourceUrl = $"{_cdnUrl}/{TemplatesPackageFileName}";
            var fileTarget = Path.Combine(tempFolder, TemplatesPackageFileName);

            Fs.EnsureFolder(tempFolder);

            DownloadContent(sourceUrl, fileTarget);

            return fileTarget;
        }

        private static void DownloadContent(string sourceUrl, string file)
        {
            var wc = new WebClient();

            wc.DownloadFile(sourceUrl, file);
        }

        private static string GetAgentName()
        {
            // If running tests in VSTS concurrently in different agents avoids the collison in templates folders
            string agentName = Environment.GetEnvironmentVariable("AGENT_NAME");
            if (string.IsNullOrEmpty(agentName))
            {
                return string.Empty;
            }
            else
            {
                return $"-{agentName}";
            }
        }
    }
}
