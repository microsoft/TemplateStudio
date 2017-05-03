// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;

using EnvDTE;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class SolutionWizard : IWizard, IDisposable
    {
        private UserSelection _userSelection;
        private GenContext _context;

        public SolutionWizard()
        {
            if (!GenContext.IsInitialized)
            {
#if DEBUG
                GenContext.Bootstrap(new LocalTemplatesSource(), new VsGenShell());
#else
                GenContext.Bootstrap(new RemoteTemplatesSource(), new VsGenShell());
#endif
            }
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public async void RunFinished()
        {
            AppHealth.Current.Verbose.TrackAsync("Creating Windows Template Studio project...").FireAndForget();
            await GenController.GenerateAsync(_userSelection);
            AppHealth.Current.Verbose.TrackAsync("Generation finished").FireAndForget();

            PostGenerationActions();
        }

        private static void PostGenerationActions()
        {

            GenContext.ToolBox.Shell.ShowStatusBarMessage(StringRes.RestoringMessage);
            GenContext.ToolBox.Shell.RestorePackages();

            GenContext.ToolBox.Shell.CollapseSolutionItems();
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            if (runKind == WizardRunKind.AsNewProject || runKind == WizardRunKind.AsMultiProject)
            {
                _context = GenContext.CreateNew(replacementsDictionary);
                _userSelection = GenController.GetUserSelection();
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
