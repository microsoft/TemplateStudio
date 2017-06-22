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

using System.Collections.Generic;
using System.IO;

using EnvDTE;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class SolutionWizard : IWizard, IContextProvider
    {
        private UserSelection _userSelection;
        private Dictionary<string, string> _replacementsDictionary;

        public string ProjectName => _replacementsDictionary["$safeprojectname$"];

        public string OutputPath => new DirectoryInfo(_replacementsDictionary["$destinationdirectory$"]).FullName;

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

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public async void RunFinished()
        {
            AppHealth.Current.Info.TrackAsync("Creating Windows Template Studio project...").FireAndForget();
            await GenController.GenerateAsync(_userSelection);
            AppHealth.Current.Info.TrackAsync("Generation finished").FireAndForget();

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
            var solutionDirectory = replacementsDictionary["$solutiondirectory$"];

            try
            {
                if (runKind == WizardRunKind.AsNewProject || runKind == WizardRunKind.AsMultiProject)
                {
                    _replacementsDictionary = replacementsDictionary;

                    GenContext.Current = this;

                    _userSelection = GenController.GetUserSelection();
                }
            }
            catch (WizardBackoutException)
            {
                if (Directory.Exists(solutionDirectory))
                {
                    Directory.Delete(solutionDirectory, true);
                }

                throw;
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
