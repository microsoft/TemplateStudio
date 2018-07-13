// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Gen
{
    public abstract class GenShell
    {
        public abstract string GetActiveProjectName();

        public abstract string GetActiveProjectPath();

        public abstract string GetSolutionPath();

        public abstract string GetActiveProjectLanguage();

        protected abstract string GetSelectedItemPath();

        public abstract void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectGuid);

        public abstract void ShowStatusBarMessage(string message);

        public abstract void AddProjectToSolution(string projectFullPath, bool usesAnyCpu);

        public abstract void AddItems(params string[] itemsFullPath);

        public abstract void CleanSolution();

        public abstract void SaveSolution();

        public abstract void AddReferenceToProject(string projectPath, string referenceToAdd);

        public abstract string GetActiveProjectNamespace();

        public abstract void ShowTaskList();

        public abstract void OpenProjectOverview();

        public abstract void ShowModal(Window dialog);

        public abstract void CancelWizard(bool back = true);

        public abstract void WriteOutput(string data);

        public abstract void CloseSolution();

        public abstract bool IsDebuggerEnabled();

        public abstract bool IsBuildInProgress();

        public abstract Guid GetVsProjectId();

        public abstract string GetActiveProjectGuid();

        public abstract string GetActiveProjectTypeGuids();

        public abstract void OpenItems(params string[] itemsFullPath);

        public virtual void RestorePackages()
        {
        }

        public virtual void CollapseSolutionItems()
        {
        }

        public virtual void RefreshProject(string projectPath)
        {
        }

        public virtual string GetVsVersion()
        {
            return "0.0.0.0";
        }

        public virtual string GetVsVersionAndInstance()
        {
            return "0.0.0.0-i";
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

        protected static Dictionary<string, List<string>> ResolveProjectFiles(string[] itemsFullPath, bool workWithProjitemsFile = false)
        {
            Dictionary<string, List<string>> filesByProject = new Dictionary<string, List<string>>();
            foreach (var item in itemsFullPath)
            {
                var itemDirectory = Directory.GetParent(item).FullName;
                var projFile = Fs.FindFileAtOrAbove(itemDirectory, "*.*proj");
                if (string.IsNullOrEmpty(projFile))
                {
                    throw new FileNotFoundException(string.Format(StringRes.ExceptionProjectNotFound, item));
                }

                if (workWithProjitemsFile && Path.GetExtension(projFile) == ".shproj")
                {
                    projFile = projFile.Replace(".shproj", ".projitems");
                }

                if (!filesByProject.ContainsKey(projFile))
                {
                    filesByProject.Add(projFile, new List<string>() { item });
                }
                else
                {
                    filesByProject[projFile].Add(item);
                }
            }

            return filesByProject;
        }
    }
}
