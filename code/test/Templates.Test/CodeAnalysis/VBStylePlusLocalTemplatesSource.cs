// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Test
{
    public class VBStylePlusLocalTemplatesSource : LocalTemplatesSource
    {
        public VBStylePlusLocalTemplatesSource()
            : base("BuildVBStyle")
        {
        }

        public override void Extract(string source, string targetFolder)
        {
            base.Extract(source, targetFolder);

            SetVBStyleFeatureContent();
        }

        private void SetVBStyleFeatureContent()
        {
            string targetVBStyleFeaturePath = Path.Combine(FinalDestination, "Features", "VBStyleAnalysis");

            if (Directory.Exists(targetVBStyleFeaturePath))
            {
                Fs.SafeDeleteDirectory(targetVBStyleFeaturePath);
            }

            Fs.CopyRecursive(@".\TestData\VBStyleAnalysis", targetVBStyleFeaturePath, true);
        }
    }
}
