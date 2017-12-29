// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Test
{
    public class StyleCopPlusLocalTemplatesSource : LocalTemplatesSourceV2
    {
        public StyleCopPlusLocalTemplatesSource()
            : base("BldStyleCop")
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
            SetStyleCopFeatureContent(targetFolder);
            return new TemplatesContentInfo()
            {
                Version = packageInfo.Version,
                Path = targetFolder,
                Date = packageInfo.Date
            };
        }

        private void SetStyleCopFeatureContent(string targetFolder)
        {
            string targetStyleCopFeaturePath = Path.Combine(targetFolder, "Features", "StyleCop");
            if (Directory.Exists(targetStyleCopFeaturePath))
            {
                Fs.SafeDeleteDirectory(targetStyleCopFeaturePath);
            }

            Fs.CopyRecursive(@".\TestData\StyleCop", targetStyleCopFeaturePath, true);
        }
    }
}
