// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Gen
{
    public class NewItemGenController : GenController
    {
        private static Lazy<NewItemGenController> _instance = new Lazy<NewItemGenController>(() => new NewItemGenController());

        public static NewItemGenController Instance => _instance.Value;

        private NewItemGenController()
        {
            PostactionFactory = new NewItemPostActionFactory();
        }

        public async Task UnsafeGenerateNewItemAsync(TemplateType templateType, UserSelection userSelection)
        {
            var genItems = GenComposer.ComposeNewItem(userSelection).ToList();

            var chrono = Stopwatch.StartNew();
            var genResults = await GenerateItemsAsync(genItems, true);
            chrono.Stop();

            TrackTelemetry(templateType, genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.ProjectType, userSelection.Framework, userSelection.Platform);
        }

        public async Task UnsafeFinishGenerationAsync(UserSelection userSelection)
        {
            var compareResult = CompareTempGenerationWithProject();
            if (userSelection.ItemGenerationType == ItemGenerationType.GenerateAndMerge)
            {
                // BackupProjectFiles
                compareResult.SyncGeneration = true;
                await ExecuteSyncGenerationPostActionsAsync(compareResult);
            }
            else
            {
                compareResult.SyncGeneration = false;
                await ExecuteOutputGenerationPostActionsAsync(compareResult);
            }
        }

        public NewItemGenerationResult CompareOutputAndProject()
        {
            var parentGenerationOutputPath = Directory.GetParent(GenContext.Current.GenerationOutputPath).FullName;
            var parentDestinationPath = Directory.GetParent(GenContext.Current.DestinationPath).FullName;

            var compareResult = CompareTempGenerationWithProject();
            var result = new NewItemGenerationResult();
            result.NewFiles.AddRange(compareResult.NewFiles.Select(n =>
                new NewItemGenerationFileInfo(
                        n,
                        Path.Combine(parentGenerationOutputPath, n),
                        Path.Combine(parentDestinationPath, n))));

            result.ConflictingFiles.AddRange(compareResult.ConflictingFiles.Select(n =>
                new NewItemGenerationFileInfo(
                        n,
                        Path.Combine(parentGenerationOutputPath, n),
                        Path.Combine(parentDestinationPath, n))));

            result.ModifiedFiles.AddRange(compareResult.ModifiedFiles.Select(n =>
                new NewItemGenerationFileInfo(
                      n,
                      Path.Combine(parentGenerationOutputPath, n),
                      Path.Combine(parentDestinationPath, n))));

            result.UnchangedFiles.AddRange(compareResult.UnchangedFiles.Select(n =>
                new NewItemGenerationFileInfo(
                     n,
                     Path.Combine(parentGenerationOutputPath, n),
                     Path.Combine(parentDestinationPath, n))));

            result.HasChangesToApply = result.NewFiles.Any() || result.ModifiedFiles.Any() ? true : false;
            return result;
        }

        public void CleanupTempGeneration()
        {
            GenContext.Current.FailedMergePostActions.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
            GenContext.Current.Projects.Clear();
            GenContext.Current.ProjectReferences.Clear();
            GenContext.Current.ProjectItems.Clear();
            GenContext.Current.FilesToOpen.Clear();

            var directory = Directory.GetParent(GenContext.Current.GenerationOutputPath).FullName;
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

        private async Task ExecuteSyncGenerationPostActionsAsync(TempGenerationResult result)
        {
            var postActions = PostactionFactory.FindSyncGenerationPostActions(result);

            foreach (var postAction in postActions)
            {
                await postAction.ExecuteAsync();
            }

            // New files aren't listed as project file modifications so any modifications should be new package references, etc.
            if (result.ModifiedFiles.Any(f => Path.GetExtension(f).EndsWith("proj", StringComparison.OrdinalIgnoreCase)))
            {
                // Forcing a package restore so don't get warnings in the designer once addition is complete
                GenContext.ToolBox.Shell.RestorePackages();
            }
        }

        private async Task ExecuteOutputGenerationPostActionsAsync(TempGenerationResult result)
        {
            var postActions = PostactionFactory.FindOutputGenerationPostActions(result);

            foreach (var postAction in postActions)
            {
                await postAction.ExecuteAsync();
            }
        }

        private TempGenerationResult CompareTempGenerationWithProject()
        {
            var parentGenerationOutputPath = Directory.GetParent(GenContext.Current.GenerationOutputPath).FullName;
            var parentDestinationPath = Directory.GetParent(GenContext.Current.DestinationPath).FullName;
            var result = new TempGenerationResult();
            var files = Directory
                .EnumerateFiles(parentGenerationOutputPath, "*", SearchOption.AllDirectories)
                .Where(f => !Regex.IsMatch(f, MergeConfiguration.PostactionRegex) && !Regex.IsMatch(f, MergeConfiguration.FailedPostactionRegex))
                .ToList();

            foreach (var file in files)
            {
                var destFilePath = file.Replace(parentGenerationOutputPath, parentDestinationPath);
                var fileName = file.Replace(parentGenerationOutputPath + Path.DirectorySeparatorChar, string.Empty);

                var projectFileName = Path.GetFullPath(Path.Combine(parentDestinationPath, fileName));

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

        private static bool FilesAreEqual(string file, string destFilePath)
        {
            return File.ReadAllLines(file).SequenceEqual(File.ReadAllLines(destFilePath));
        }

        private static void TrackTelemetry(TemplateType templateType, IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent, string appProjectType, string appFx, string appPlatform)
        {
            try
            {
                int pagesAdded = genItems.Count(t => t.Template.GetTemplateType() == TemplateType.Page);
                int featuresAdded = genItems.Count(t => t.Template.GetTemplateType() == TemplateType.Feature);
                var pageIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Select(t => t.Template.Identity));
                var featureIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Select(t => t.Template.Identity));

                AppHealth.Current.Telemetry.TrackNewItemAsync(templateType, appProjectType, appFx, appPlatform, GenContext.ToolBox.Shell.GetVsProjectId(), pagesAdded, featuresAdded, pageIdentities, featureIdentities, timeSpent).FireAndForget();

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
    }
}
