// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Templates.Core;
using Xunit;

namespace Microsoft.UI.Test.VisualTests
{
    [Collection("UI-Visuals")]
    [Trait("ExecutionSet", "ManualOnly")]
    [Trait("Type", "WinAppDriver")]
    public class WizardLocalizationChecks : AutomatedWizardTestingBase
    {
        [Fact]
        public void EnsureLaunchPageVisualsAreEquivalentAsync()
        {
            var errorOutput = new List<string>();

            var defaultText = GetDefaultText();

            foreach (var culture in NonDefaultVsCultures)
            {
                var localizedText = GetAllUiText(culture);

                foreach (var page in defaultText)
                {
                    foreach (var defaultString in page.Value)
                    {
                        if (localizedText[page.Key].Contains(defaultString))
                        {
                            if (!IsKnownLocalizationException(culture, page.Key, defaultString))
                            {
                                errorOutput.Add($"In {culture}, page {page.Key} includes unlocalized text '{defaultString}'");
                            }
                        }
                    }
                }
            }

            Assert.True(errorOutput.Count == 0, string.Join(Environment.NewLine, errorOutput));
        }

        public bool IsKnownLocalizationException(string culture, string page, string textContent)
        {
            var textValidOnAnyPage = new[] { "http://aka.ms/wts", "Windows Template Studio", "Microsoft.Toolkit.Uwp" };

            // TODO [ML]: add per page exceptions
            // TODO [ML]: add per culture AND page exceptions
            if (textValidOnAnyPage.Contains(textContent))
            {
                return true;
            }

            return false;
        }

        private Dictionary<string, List<string>> GetDefaultText()
        {
            return GetAllUiText();
        }

        private Dictionary<string, List<string>> GetAllUiText(string culture = "en-US")
        {
            var result = new Dictionary<string, List<string>>();

            ForEachPageInProjectWizard(culture, ProgrammingLanguages.CSharp, false, pageName =>
            {
                result.Add(pageName, GetAllTextOnScreen());
            });

            return result;
        }

        private List<string> GetAllTextOnScreen()
        {
            var result = new List<string>();

            foreach (var element in WizardSession.FindElementsByClassName("TextBlock"))
            {
                result.Add(element.Text);
            }

            return result;
        }
    }
}
