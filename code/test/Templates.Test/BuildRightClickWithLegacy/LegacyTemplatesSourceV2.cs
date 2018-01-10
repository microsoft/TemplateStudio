// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Test
{
    public sealed class LegacyTemplatesSourceV2 : RemoteTemplatesSource
    {
        public override string Id => "TestLegacy" + GetAgentName();

        public override TemplatesContentInfo GetContent(TemplatesPackageInfo packageInfo, string workingFolder)
        {
            LoadConfig();
            var package = Config.Latest;
            Acquire(ref package);
            return base.GetContent(package, workingFolder);
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
