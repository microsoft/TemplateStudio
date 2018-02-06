// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.UI.Resources;
using Microsoft.VisualStudio.TemplateWizard;
using Newtonsoft.Json;

namespace Microsoft.Templates.UI
{
    public class NewItemGenController : GenController
    {
        private static Lazy<NewItemGenController> _instance = new Lazy<NewItemGenController>(Initialize);

        public static NewItemGenController Instance => _instance.Value;

        private static NewItemGenController Initialize()
        {
            return new NewItemGenController(new NewItemPostActionFactory());
        }

        private NewItemGenController(PostActionFactory postactionFactory)
        {
            PostactionFactory = postactionFactory;
        }

        public UserSelection GetUserSelectionNewFeature(string language)
        {
            var newItem = new Views.NewItem.WizardShell(TemplateType.Feature, language);
            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(newItem);
                if (newItem.Result != null)
                {
                    TrackWizardCompletedTelemery(WizardTypeEnum.AddFeature, newItem.Result.ItemGenerationType);

                    return newItem.Result;
                }
                else
                {
                    AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.AddFeature, GenContext.ToolBox.Shell.GetVsVersion()).FireAndForget();
                }
            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                newItem.SafeClose();
                ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return null;
        }

        public UserSelection GetUserSelectionNewPage(string language)
        {
            var newItem = new Views.NewItem.WizardShell(TemplateType.Page, language);
            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(newItem);
                if (newItem.Result != null)
                {
                    TrackWizardCompletedTelemery(WizardTypeEnum.AddPage, newItem.Result.ItemGenerationType);

                    return newItem.Result;
                }
                else
                {
                    AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.AddPage, GenContext.ToolBox.Shell.GetVsVersion()).FireAndForget();
                }
            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                newItem.SafeClose();
                ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return null;
        }

        public async Task GenerateNewItemAsync(TemplateType templateType, UserSelection userSelection)
        {
            try
            {
               await UnsafeGenerateNewItemAsync(templateType, userSelection);
            }
            catch (Exception ex)
            {
                ShowError(ex, userSelection);

                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public async Task UnsafeGenerateNewItemAsync(TemplateType templateType, UserSelection userSelection)
        {
            var genItems = GenComposer.ComposeNewItem(userSelection).ToList();
            var chrono = Stopwatch.StartNew();

            var genResults = await GenerateItemsAsync(genItems);

            chrono.Stop();

            TrackTelemery(templateType, genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.ProjectType, userSelection.Framework);
        }

        private TempGenerationResult CompareTempGenerationWithProject()
        {
            var result = new TempGenerationResult();
            var files = Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*", SearchOption.AllDirectories)
                .Where(f => !Regex.IsMatch(f, MergeConfiguration.PostactionRegex) && !Regex.IsMatch(f, MergeConfiguration.FailedPostactionRegex))
                .ToList();

            foreach (var file in files)
            {
                var destFilePath = file.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath);
                var fileName = file.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);

                var projectFileName = Path.GetFullPath(Path.Combine(GenContext.Current.ProjectPath, fileName));

                if (File.Exists(projectFileName))
                {
                    if (GenContext.Current.MergeFilesFromProject.ContainsKey(fileName))
                    {
                        if (FilesAreEqual(file, destFilePath))
                        {
                            if (!GenContext.Current.FailedMergePostActions.Any(g => g.FileName == fileName))
                            {
                                GenContext.Current.MergeFilesFromProject.Remove(fileName);
                                result.UnchangedFiles.Add(fileName);
                            }
                        }
                        else
                        {
                            result.ModifiedFiles.Add(fileName);
                        }
                    }
                    else
                    {
                        if (FilesAreEqual(file, destFilePath))
                        {
                            result.UnchangedFiles.Add(fileName);
                        }
                        else
                        {
                            result.ConflictingFiles.Add(fileName);
                        }
                    }
                }
                else
                {
                    result.NewFiles.Add(fileName);
                }
            }

            return result;
        }

        public NewItemGenerationResult CompareOutputAndProject()
        {
            var compareResult = CompareTempGenerationWithProject();
            var result = new NewItemGenerationResult();
            result.NewFiles.AddRange(compareResult.NewFiles.Select(n =>
                new NewItemGenerationFileInfo(
                        n,
                        Path.Combine(GenContext.Current.OutputPath, n),
                        Path.Combine(GenContext.Current.ProjectPath, n))));

            result.ConflictingFiles.AddRange(compareResult.ConflictingFiles.Select(n =>
                new NewItemGenerationFileInfo(
                        n,
                        Path.Combine(GenContext.Current.OutputPath, n),
                        Path.Combine(GenContext.Current.ProjectPath, n))));

            result.ModifiedFiles.AddRange(compareResult.ModifiedFiles.Select(n =>
                new NewItemGenerationFileInfo(
                      n,
                      Path.Combine(GenContext.Current.OutputPath, n),
                      Path.Combine(GenContext.Current.ProjectPath, n))));

            result.UnchangedFiles.AddRange(compareResult.UnchangedFiles.Select(n =>
                new NewItemGenerationFileInfo(
                     n,
                     Path.Combine(GenContext.Current.OutputPath, n),
                     Path.Combine(GenContext.Current.ProjectPath, n))));

            result.HasChangesToApply = result.NewFiles.Any() || result.ModifiedFiles.Any() ? true : false;
            return result;
        }

        private static bool FilesAreEqual(string file, string destFilePath)
        {
            return File.ReadAllLines(file).SequenceEqual(File.ReadAllLines(destFilePath));
        }

        public void FinishGeneration(UserSelection userSelection)
        {
            try
            {
                UnsafeFinishGeneration(userSelection);
            }
            catch (Exception ex)
            {
                ShowError(ex, userSelection);
                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public void UnsafeFinishGeneration(UserSelection userSelection)
        {
            var compareResult = CompareTempGenerationWithProject();
            if (userSelection.ItemGenerationType == ItemGenerationType.GenerateAndMerge)
            {
                // BackupProjectFiles
                compareResult.SyncGeneration = true;
                ExecuteSyncGenerationPostActions(compareResult);
            }
            else
            {
                compareResult.SyncGeneration = false;
                ExecuteOutputGenerationPostActions(compareResult);
            }
        }

        public void CleanupTempGeneration()
        {
            GenContext.Current.FailedMergePostActions.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
            GenContext.Current.ProjectItems.Clear();
            GenContext.Current.FilesToOpen.Clear();

            var directory = GenContext.Current.OutputPath;
            try
            {
                if (directory.Contains(Path.GetTempPath()))
                {
                    Fs.SafeDeleteDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                var msg = $"The folder {directory} can't be delete. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
            }
        }

        private void BackupProjectFiles(TempGenerationResult result)
        {
            var projectGuid = GenContext.ToolBox.Shell.GetActiveProjectGuid();

            if (string.IsNullOrEmpty(projectGuid))
            {
                return;
            }

            var backupFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Configuration.Current.BackupFolderPath,
                projectGuid);

            var fileName = Path.Combine(backupFolder, "backup.json");

            if (Directory.Exists(backupFolder))
            {
                Fs.SafeDeleteDirectory(backupFolder);
            }

            Fs.EnsureFolder(backupFolder);

            File.WriteAllText(fileName, JsonConvert.SerializeObject(result), Encoding.UTF8);

            var modifiedFiles = result.ConflictingFiles.Concat(result.ModifiedFiles);

            foreach (var file in modifiedFiles)
            {
                var originalFile = Path.Combine(GenContext.Current.ProjectPath, file);
                var backupFile = Path.Combine(backupFolder, file);
                var destDirectory = Path.GetDirectoryName(backupFile);

                Fs.SafeCopyFile(originalFile, destDirectory, true);
            }
        }

        private void ExecuteSyncGenerationPostActions(TempGenerationResult result)
        {
            var postActions = PostactionFactory.FindSyncGenerationPostActions(result);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }

            // New files aren't listed as project file modifications so any modifications should be new package references, etc.
            if (result.ModifiedFiles.Any(f => Path.GetExtension(f).EndsWith("proj", StringComparison.OrdinalIgnoreCase)))
            {
                // Forcing a package restore so don't get warnings in the designer once addition is complete
                GenContext.ToolBox.Shell.RestorePackages();
            }
        }

        private void ExecuteOutputGenerationPostActions(TempGenerationResult result)
        {
            var postActions = PostactionFactory.FindOutputGenerationPostActions(result);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void TrackTelemery(TemplateType templateType, IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent, string appProjectType, string appFx)
        {
            try
            {
                int pagesAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Count();
                int featuresAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Count();
                var pageIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Select(t => t.Template.Identity));
                var featureIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Select(t => t.Template.Identity));

                AppHealth.Current.Telemetry.TrackNewItemAsync(templateType, appProjectType, appFx, GenContext.ToolBox.Shell.GetVsProjectId(), pagesAdded, featuresAdded, pageIdentities, featureIdentities, timeSpent).FireAndForget();

                foreach (var genInfo in genItems)
                {
                    if (genInfo.Template == null)
                    {
                        continue;
                    }

                    string resultsKey = $"{genInfo.Template.Identity}_{genInfo.Name}";

                    AppHealth.Current.Telemetry.TrackItemGenAsync(genInfo.Template, GenSourceEnum.NewItem, appProjectType, appFx, genResults[resultsKey]).FireAndForget();
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, StringRes.ErrorTrackTelemetryException).FireAndForget();
            }
        }

        private static void TrackWizardCompletedTelemery(WizardTypeEnum wizardType, ItemGenerationType generationType)
        {
            switch (generationType)
            {
                case ItemGenerationType.Generate:
                    AppHealth.Current.Telemetry.TrackWizardCompletedAsync(wizardType, WizardActionEnum.GenerateItem, GenContext.ToolBox.Shell.GetVsVersion()).FireAndForget();

                    break;
                case ItemGenerationType.GenerateAndMerge:
                    AppHealth.Current.Telemetry.TrackWizardCompletedAsync(wizardType, WizardActionEnum.GenerateAndMergeItem, GenContext.ToolBox.Shell.GetVsVersion()).FireAndForget();
                    break;
                default:
                    break;
            }
        }
    }
}
