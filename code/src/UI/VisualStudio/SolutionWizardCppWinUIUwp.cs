// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core;
using Microsoft.Templates.UI;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class SolutionWizardCppWinUIUwp : SolutionWizard
    {
        public SolutionWizardCppWinUIUwp()
        {
            Initialize(Platforms.WinUI, ProgrammingLanguages.Cpp, AppModels.Uwp);
        }
    }
}
