// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Locations;

namespace Microsoft.UI.Test.ProjectConfigurationTests
{
    public sealed class UITestsTemplatesSource : LocalTemplatesSource
    {
        public UITestsTemplatesSource(string installedPackagePath)
            : base(installedPackagePath)
        {
        }

        public override string Id => "UITest" + GetAgentName();

        protected override string Origin => $@"..\..\..\UI.Test\TestData\{TemplatesFolderName}";
    }
}
