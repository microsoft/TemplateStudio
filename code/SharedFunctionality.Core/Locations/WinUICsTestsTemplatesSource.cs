// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Microsoft.Templates.Core.Locations
{
    public sealed class WinUICsTestsTemplatesSource : LocalTemplatesSource
    {
        public WinUICsTestsTemplatesSource()
            : base(null)
        {
        }

        public WinUICsTestsTemplatesSource(string id)
            : base(string.Empty, id)
        {
        }

        public override string Id => "WinUICsTest" + GetAgentName();

        public override string GetContentRootFolder()
        {
            var dir = Path.GetDirectoryName(System.Environment.CurrentDirectory);
            dir = Path.Combine(dir, @"..\..\..\TemplateStudioForWinUICs\Templates\WinUI");

            return dir;
        }
    }
}
