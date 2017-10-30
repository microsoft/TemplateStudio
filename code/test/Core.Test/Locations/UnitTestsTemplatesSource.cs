// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Core.Test.Locations
{
    public sealed class UnitTestsTemplatesSource : TemplatesSource
    {
        private string _localVersion = "0.0.0.0";

        public override string Id => "UnitT_" + System.Threading.Thread.CurrentThread.ManagedThreadId + GetAgentName();

        protected override bool VerifyPackageSignatures => false;

        public override bool ForcedAcquisition => true;

        protected override string AcquireMstx()
        {
            // Hack and return the tempfolder directly to skip zipping and unzipping
            return $@"..\..\TestData\{SourceFolderName}";
        }

        public override void Extract(string source, string targetFolder)
        {
            SetLocalContent(source, targetFolder, new Version(_localVersion));
        }

        private void SetLocalContent(string sourcePath, string finalTargetFolder, Version version)
        {
            Version ver = version;

            string finalDestination = PrepareFinalDestination(finalTargetFolder, ver);

            if (!Directory.Exists(finalDestination))
            {
                Fs.CopyRecursive(sourcePath, finalDestination, true);
            }
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
