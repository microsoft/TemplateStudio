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

        public abstract bool SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectGuid);

        public abstract void ShowStatusBarMessage(string message);

        public abstract void AddProjectToSolution(string projectFullPath);

        public abstract void AddItems(params string[] itemsFullPath);

        public abstract void CleanSolution();

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

        public abstract string GetActiveProjectTypeGuids();

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

        public string GetPlatform()
        {
            if (IsXamarin())
            {
                return Platforms.Xamarin;
            }

            if (IsUwp())
            {
                return Platforms.Uwp;
            }

            throw new Exception(StringRes.ExceptionUnableResolvePlatform);
        }

        protected static Dictionary<string, List<string>> ResolveProjectFiles(string[] itemsFullPath)
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

                if (Path.GetExtension(projFile) == ".shproj")
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

        private bool IsXamarin()
        {
            var searchPath = new DirectoryInfo(GetActiveProjectPath()).Parent.FullName;
            string[] fileExtensions = { ".json", ".config", ".csproj" };

            var files = Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => fileExtensions.Contains(Path.GetExtension(f)));

            foreach (string file in files)
            {
                if (File.ReadAllText(file).Contains("Xamarin.Forms"))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsUwp()
        {
            var projectTypeGuids = GenContext.ToolBox.Shell.GetActiveProjectTypeGuids();

            if (projectTypeGuids.ToUpperInvariant().Split(';').Contains("{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
