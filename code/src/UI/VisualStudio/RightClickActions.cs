// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
                OutputPath = GenContext.GetTempGenerationPath(ProjectName);
                ProjectItems = new List<string>();
                FilesToOpen = new List<string>();
                FailedMergePostActions = new List<FailedMergePostAction>();
                MergeFilesFromProject = new Dictionary<string, List<MergeInfo>>();

                GenContext.Current = this;
            }
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

        public bool Visible()
        {
            return GenContext.ToolBox.Shell.GetActiveProjectIsWts();
        }

        public bool Enabled()
        {
            return !GenContext.ToolBox.Shell.IsDebuggerEnabled();
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
            var projectGuid = GenContext.ToolBox.Shell.GetActiveProjectGuid();
            return Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath, projectGuid.ToString());
        }

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Count() > 0;
        }
    }
}
