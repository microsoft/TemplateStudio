// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;

using Xunit;

namespace Microsoft.Templates.Core.Test.Locations
{
    [Trait("ExecutionSet", "Minimum")]

    public class TemplateSourceConfigTests
    {
        [Fact]
        public void TemplateSourceConfigTest_LoadFromFile()
        {
            var configFile = Path.GetFullPath(@".\Packaging\SampleConfig.json");

            var config = TemplatesSourceConfig.LoadFromFile(configFile);

            Assert.Equal(new Version(1, 2, 3, 4), config.Latest.Version);
            Assert.Equal("CS", config.Latest.Language);
            Assert.Equal("Uwp", config.Latest.Platform);
            Assert.Equal(new Version(1, 2), config.Latest.WizardVersions[0]);
            Assert.Equal(2, config.Versions.Count());
        }

        [Fact]
        public void TemplateSourceConfigTest_ResolveFile()
        {
            var configFile = Path.GetFullPath(@".\Packaging\SampleConfig.json");

            var config = TemplatesSourceConfig.LoadFromFile(configFile);

            var package = config.ResolvePackage(new Version(1, 3), Platforms.Uwp, ProgrammingLanguages.GetShortProgrammingLanguage(ProgrammingLanguages.CSharp));

            Assert.Equal("UWP.CS.Templates_1.2.3.4.mstx", package.Name);
        }
    }
}
