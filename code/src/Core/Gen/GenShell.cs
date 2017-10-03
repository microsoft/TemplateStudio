// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Windows;

namespace Microsoft.Templates.Core.Gen
{
    public abstract class GenShell
    {
        public abstract string GetActiveProjectName();
        public abstract string GetActiveProjectPath();
        public abstract string GetActiveProjectLanguage();
        protected abstract string GetSelectedItemPath();

        public abstract bool SetActiveConfigurationAndPlatform(string configurationName, string platformName);
        public abstract void ShowStatusBarMessage(string message);
        public abstract void AddProjectToSolution(string projectFullPath);
        public abstract void AddItems(params string[] itemsFullPath);
        public abstract void SaveSolution();
        public abstract string GetActiveProjectNamespace();
        public abstract void ShowTaskList();
        public abstract void ShowModal(Window dialog);
        public abstract void CancelWizard(bool back = true);
        public abstract void WriteOutput(string data);
        public abstract void CloseSolution();
        public abstract bool IsDebuggerEnabled();

        public abstract Guid GetVsProjectId();

        public abstract string GetActiveProjectGuid();

        public abstract void OpenItems(params string[] itemsFullPath);

        public virtual void RestorePackages()
        {
        }

        public virtual void CollapseSolutionItems()
        {
        }

        public virtual void RefreshProject()
        {
        }

        public virtual string GetVsVersion()
        {
            return "0.0.0.0";
        }

        public bool GetActiveProjectIsWts()
        {
            bool result = false;
            var activeProjectPath = GetActiveProjectPath();
            if (!string.IsNullOrEmpty(activeProjectPath))
            {
                var appManifestFilePath = Path.Combine(activeProjectPath, "Package.appxmanifest");
                if (File.Exists(appManifestFilePath))
                {
                    var fileContent = File.ReadAllText(appManifestFilePath);
                    result = fileContent.Contains("genTemplate:Metadata");
                }
            }
            return result;
        }
    }
}
