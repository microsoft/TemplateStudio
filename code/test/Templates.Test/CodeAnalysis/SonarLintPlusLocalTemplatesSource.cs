// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Test
{
    public class SonarLintPlusLocalTemplatesSource : LocalTemplatesSourceV2
    {
        public SonarLintPlusLocalTemplatesSource()
            : base("BuildSonarLint")
        {
        }

        public override TemplatesContentInfo GetContent(TemplatesPackageInfo packageInfo, string workingFolder)
        {
            string targetFolder = Path.Combine(workingFolder, packageInfo.Version.ToString());

            if (Directory.Exists(targetFolder))
            {
                Fs.SafeDeleteDirectory(targetFolder);
            }

            Fs.CopyRecursive(packageInfo.LocalPath, targetFolder, true);
            SetSonarLintFeatureContent(targetFolder);
            return new TemplatesContentInfo()
            {
                Version = packageInfo.Version,
                Path = targetFolder,
                Date = packageInfo.Date
            };
        }

        private void SetSonarLintFeatureContent(string targetFolder)
        {
            string targetSonarLintFeaturePath = Path.Combine(targetFolder, "Features", "SonarLintVB");
            if (Directory.Exists(targetSonarLintFeaturePath))
            {
                Fs.SafeDeleteDirectory(targetSonarLintFeaturePath);
            }

            Fs.CopyRecursive(@".\TestData\SonarLintVB", targetSonarLintFeaturePath, true);
        }
    }
}
