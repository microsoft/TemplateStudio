// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Microsoft.Templates.Core.Locations
{
    public sealed class UwpTestsTemplatesSource : LocalTemplatesSource
    {
        public UwpTestsTemplatesSource()
            : base(null)
        {
        }

        public UwpTestsTemplatesSource(string id)
            : base(string.Empty, id)
        {
        }

        public override string Id => "UwpUITest" + GetAgentName();

        public override string GetContentRootFolder()
        {
            var dir = Path.GetDirectoryName(System.Environment.CurrentDirectory);
            dir = Path.Combine(dir, @"..\..\..\TemplateStudioForUwp\Templates\Uwp");

            return dir;
        }
    }
}
