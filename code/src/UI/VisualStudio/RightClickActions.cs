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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.UI.Resources;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class RightClickActions : IContextProvider
    {
        private string _language;

        public string ProjectName { get; private set; }

        public string OutputPath { get; private set; }

        public string ProjectPath { get; private set; }

        public List<string> ProjectItems { get; private set; }

        public List<string> FilesToOpen { get; private set; }

        public List<FailedMergePostAction> FailedMergePostActions { get; private set; }

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; private set; }

        public RightClickActions(string language)
        {
            _language = language;

            if (GenContext.InitializedLanguage != language)
            {
#if DEBUG
                GenContext.Bootstrap(new LocalTemplatesSource(), new VsGenShell(), _language);
#else
                GenContext.Bootstrap(new RemoteTemplatesSource(), new VsGenShell(), _language);
#endif
            }
        }

        public void AddNewPage()
        {
            if (GenContext.ToolBox.Shell.GetActiveProjectIsWts())
            {
                SetContext();

                try
                {
                    var userSelection = NewItemGenController.Instance.GetUserSelectionNewPage();

                    if (userSelection != null)
                    {
                        NewItemGenController.Instance.FinishGeneration(userSelection);
                        GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.NewItemAddPageSuccessStatusMsg, userSelection.Pages[0].name));
                    }
                }
                catch (WizardBackoutException)
                {
                    GenContext.ToolBox.Shell.ShowStatusBarMessage(StringRes.NewItemAddPageCancelled);
                }
            }
        }

        private void SetContext()
        {
            if (GenContext.InitializedLanguage == _language)
            {
                ProjectPath = GenContext.ToolBox.Shell.GetActiveProjectPath();
                ProjectName = GenContext.ToolBox.Shell.GetActiveProjectName();
                OutputPath = GetTempGenerationPath(ProjectName);
                ProjectItems = new List<string>();
                FilesToOpen = new List<string>();
                FailedMergePostActions = new List<FailedMergePostAction>();
                MergeFilesFromProject = new Dictionary<string, List<MergeInfo>>();

                GenContext.Current = this;
            }
        }

        private static string GetTempGenerationPath(string projectName)
        {
            var tempGenerationPath = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);
            Fs.EnsureFolder(tempGenerationPath);

            var tempGenerationName = $"{projectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
            var inferredName = Naming.Infer(tempGenerationName, new List<Validator>() { new DirectoryExistsValidator(tempGenerationPath) }, "_");

            return Path.Combine(tempGenerationPath, inferredName);
        }

        public void AddNewFeature()
        {
            if (GenContext.ToolBox.Shell.GetActiveProjectIsWts())
            {
                SetContext();
                try
                {
                    var userSelection = NewItemGenController.Instance.GetUserSelectionNewFeature();

                    if (userSelection != null)
                    {
                        NewItemGenController.Instance.FinishGeneration(userSelection);
                        GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.NewItemAddFeatureSuccessStatusMsg, userSelection.Features[0].name));
                    }
                }
                catch (WizardBackoutException)
                {
                    GenContext.ToolBox.Shell.ShowStatusBarMessage(StringRes.NewItemAddFeatureCancelled);
                }
            }
        }

        public bool Enabled()
        {
            return GenContext.ToolBox.Shell.GetActiveProjectIsWts();
        }

        public void OpenTempFolder()
        {
            var tempPath = GetTempGenerationFolder();
            if (HasContent(tempPath))
            {
                System.Diagnostics.Process.Start(tempPath);
            }
        }
        public bool TempFolderAvailable()
        {
            return HasContent(GetTempGenerationFolder());
        }

        private static string GetTempGenerationFolder()
        {
            return Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);
        }

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Count() > 0;
        }
    }
}
