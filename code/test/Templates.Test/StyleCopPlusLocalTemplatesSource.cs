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
    public class StyleCopPlusLocalTemplatesSource : LocalTemplatesSource
    {
        public StyleCopPlusLocalTemplatesSource()
            : base("BuildStyleCop")
        {
        }

        public override void Extract(string source, string targetFolder)
        {
            base.Extract(source, targetFolder);

            SetStyleCopFeatureContent();
        }

        private void SetStyleCopFeatureContent()
        {
            string targetStyleCopFeaturePath = Path.Combine(FinalDestination, "Features", "StyleCop");
            if (Directory.Exists(targetStyleCopFeaturePath))
            {
                Fs.SafeDeleteDirectory(targetStyleCopFeaturePath);
            }

            Fs.CopyRecursive(@".\TestData\StyleCop", targetStyleCopFeaturePath, true);
        }
    }
}
