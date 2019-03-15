// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Gen
{
    public abstract class GenShell
    {
        public abstract string GetActiveProjectName();

        public abstract string GetActiveProjectPath();

        public abstract string GetActiveProjectLanguage();

        public abstract void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectGuid);

        public abstract void ShowStatusBarMessage(string message);

        public abstract void AddContextItemsToSolution(ProjectInfo projectInfo);

        public abstract string GetActiveProjectNamespace();

        public abstract void ShowTaskList();

        public abstract void OpenProjectOverview();

        public abstract void ShowModal(IWindow shell);

        public abstract void CancelWizard(bool back = true);

        public abstract void WriteOutput(string data);

        public abstract void CloseSolution();

        public abstract bool IsDebuggerEnabled();

        public abstract bool IsBuildInProgress();

        public abstract Guid GetVsProjectId();

        public abstract string GetActiveProjectGuid();

        public abstract string GetActiveProjectTypeGuids();

        public abstract void OpenItems(params string[] itemsFullPath);

        public virtual void CollapseSolutionItems()
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

        public abstract VSTelemetryInfo GetVSTelemetryInfo();

        public abstract void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pages, string features, Dictionary<string, double> metrics, bool success = true);

        public abstract void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pages, string features, Dictionary<string, double> metrics, bool success = true);

        public abstract void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true);

        protected static Dictionary<string, List<string>> ResolveProjectFiles(IEnumerable<string> itemsFullPath, bool workWithProjitemsFile = false)
        {
            Dictionary<string, List<string>> filesByProject = new Dictionary<string, List<string>>();
            foreach (var item in itemsFullPath)
            {
                var itemDirectory = Directory.GetParent(item).FullName;
                var projFile = Fs.FindFileAtOrAbove(itemDirectory, "*.*proj");
                if (string.IsNullOrEmpty(projFile))
                {
                    AppHealth.Current.Error.TrackAsync(string.Format(StringRes.ExceptionProjectNotFound, item)).FireAndForget();
                }
                else
                {
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
            }

            return filesByProject;
        }

        public abstract string CreateCertificate(string publisherName);
    }
}
