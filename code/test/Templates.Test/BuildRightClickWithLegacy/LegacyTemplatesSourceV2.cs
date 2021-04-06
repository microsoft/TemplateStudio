// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Utilities.Services;

namespace Microsoft.Templates.Test.BuildWithLegacy
{
    public sealed class LegacyTemplatesSourceV2 : RemoteTemplatesSource
    {
        public override string Id => "TestLegacy" + GetAgentName();

        public LegacyTemplatesSourceV2(string platform, string language)
           : base(platform, language, string.Empty, new DigitalSignatureService())
        {
        }

        public override async Task<TemplatesContentInfo> GetContentAsync(TemplatesPackageInfo packageInfo, string workingFolder, CancellationToken ct)
        {
            await AcquireAsync(packageInfo, ct);

            var templatecontent = await base.GetContentAsync(packageInfo, workingFolder, ct);

            return templatecontent;
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
