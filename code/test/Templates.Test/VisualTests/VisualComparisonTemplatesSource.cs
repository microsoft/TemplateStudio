// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Test.VisualTests
{
    public class VisualComparisonTemplatesSource : LocalTemplatesSource
    {
        public VisualComparisonTemplatesSource()
            : base("VisualComp")
        {
        }

        public override void Extract(string source, string targetFolder)
        {
            Fs.CopyRecursive(@".\VisualTests\Templates", targetFolder, true);
        }
    }
}
