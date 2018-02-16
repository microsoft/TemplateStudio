// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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
            var defaultText = GetDefaultText();

            foreach (var culture in NonDefaultVsCultures)
            {
                var localizedText = GetAllUiText(culture);

                // compare localizedText with defaultText
            }
        }

        private Dictionary<string, string> GetDefaultText()
        {
            return GetAllUiText();
        }

        private Dictionary<string, string> GetAllUiText(string culture = "")
        {
            var result = new Dictionary<string, string>();

            // TODO [ML]: implement this (GetAllUiText)
            ForEachPageInProjectWizard(culture, ProgrammingLanguages.CSharp, false, pageName =>
            {
                result.Add(pageName, "XXXX");
            });

            return result;
        }
    }
}
