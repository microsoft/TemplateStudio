// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Xunit;

namespace Microsoft.UI.Test.VisualTests
{
    // The methods in this class aren't real tests, they just use the test infrastructure to captures screenshots for manual review
    [Collection("UI-Visuals")]
    [Trait("ExecutionSet", "ManualOnly")]
    [Trait("Type", "WinAppDriver")]
    public class CaptureWizardScreenshots : AutomatedWizardTestingBase
    {
        [Trait("ExecutionSet", "ManualOnly")]
        [Fact]
        public void GetBasicScreenshots_NewProject_CSandVB_AllCultures()
        {
            var testOutputRoot = GetRootFolderForTestOutput();

            foreach (var culture in AllVsCultures)
            {
                foreach (var progLang in ProgrammingLanguageVBAndCharp)
                {
                    ForEachPageInProjectWizard(culture, progLang, includeDetails: false, action: pageName =>
                    {
                        TakeScreenshot(Path.Combine(testOutputRoot, $"{culture}_{progLang}_{Uri.EscapeUriString(pageName)}.png"));
                    });
                }
            }

            Assert.NotEmpty(Directory.GetFiles(testOutputRoot));
        }

        [Trait("ExecutionSet", "ManualOnly")]
        [Fact]
        public void GetScreenshots_AddPage_CSandVB_AllCultures()
        {
            var testOutputRoot = GetRootFolderForTestOutput();

            foreach (var culture in AllVsCultures)
            {
                foreach (var progLang in ProgrammingLanguageVBAndCharp)
                {
                    ForEachStepInAddPageWizard(culture, progLang, includeDetails: false, action: pageName =>
                    {
                        TakeScreenshot(Path.Combine(testOutputRoot, $"{culture}_{progLang}_{Uri.EscapeUriString(pageName)}.png"));
                    });
                }
            }

            Assert.NotEmpty(Directory.GetFiles(testOutputRoot));
        }

        [Trait("ExecutionSet", "ManualOnly")]
        [Fact]
        public void GetScreenshots_AddFeature_CSandVB_AllCultures()
        {
            var testOutputRoot = GetRootFolderForTestOutput();

            foreach (var culture in AllVsCultures)
            {
                foreach (var progLang in ProgrammingLanguageVBAndCharp)
                {
                    ForEachStepInAddFeatureWizard(culture, progLang, includeDetails: false, action: pageName =>
                    {
                        TakeScreenshot(Path.Combine(testOutputRoot, $"{culture}_{progLang}_{Uri.EscapeUriString(pageName)}.png"));
                    });
                }
            }

            Assert.NotEmpty(Directory.GetFiles(testOutputRoot));
        }
    }
}
