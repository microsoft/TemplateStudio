// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Locations;

namespace Microsoft.UI.Test.ProjectConfigurationTests
{
    public sealed class UITestsTemplatesSource : LocalTemplatesSource
    {
        public UITestsTemplatesSource()
            : base(null)
        {
        }

        public override string Id => "UITest" + GetAgentName();

        public override string GetContentRootFolder()
        {
            var dir = System.IO.Path.GetDirectoryName(System.Environment.CurrentDirectory);

            dir = System.IO.Path.Combine(dir, @"..\..\UI.Test\TestData\Templates\test");

            return dir;
        }
    }
}
