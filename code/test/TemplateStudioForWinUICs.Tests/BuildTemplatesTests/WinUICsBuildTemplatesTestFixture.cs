// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Test
{
    public class WinUICsBuildTemplatesTestFixture : BuildTemplatesTestFixture
    {
        public override TemplatesSource Source => new WinUICsTestsTemplatesSource();

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForBuild(string frameworkFilter, string language = ProgrammingLanguages.CSharp, string platform = Platforms.WinUI, string excludedItem = "")
        {
            InitializeTemplates(new WinUICsTestsTemplatesSource(ShortFrameworkName(frameworkFilter)), language);

            return BaseGenAndBuildFixture.GetPageAndFeatureTemplates(frameworkFilter, language, platform, excludedItem);
        }
    }
}
