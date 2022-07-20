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
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.SharedResources;

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
            VerifyGenContextPaths();
            ValidateUserSelection(userSelection, false);

            var genItems = GenComposer.ComposeNewItem(userSelection).ToList();

            var chrono = Stopwatch.StartNew();
            var genResults = await GenerateItemsAsync(genItems);
            chrono.Stop();

            TrackTelemetry(templateType, genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.Context);
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
            GenContext.Current.ProjectInfo.Projects.Clear();
            GenContext.Current.ProjectInfo.NugetReferences.Clear();
            GenContext.Current.ProjectInfo.SdkReferences.Clear();
            GenContext.Current.ProjectInfo.ProjectReferences.Clear();
            GenContext.Current.ProjectInfo.ProjectItems.Clear();
            GenContext.Current.ProjectInfo.ProjectConfigurations.Clear();
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

        private void ExecuteSyncGenerationPostActions(TempGenerationResult result)
        {
            var postActions = PostactionFactory.FindSyncGenerationPostActions(result);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
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
                var destFilePath = file.GetDestinationPath();
                var fileName = file.GetPathRelativeToGenerationParentPath();

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

        private static void TrackTelemetry(TemplateType templateType, IEnumerable<GenInfo> genItems, Dictionary<string, ITemplateCreationResult> genResults, double timeSpent, UserSelectionContext context)
        {
            try
            {
                var genItemsTelemetryData = new GenItemsTelemetryData(genItems);
                AppHealth.Current.Telemetry.TrackNewItemAsync(templateType, context, GenContext.ToolBox.Shell.Project.GetProjectGuidByName(GenContext.Current.ProjectName), genItemsTelemetryData, timeSpent).FireAndForget();

                foreach (var genInfo in genItems.Where(g => g.Template != null))
                {
                    string resultsKey = $"{genInfo.Template.Identity}_{genInfo.Name}";
                    AppHealth.Current.Telemetry.TrackItemGenAsync(genInfo.Template, GenSourceEnum.NewItem, context, genResults[resultsKey]).FireAndForget();
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, Resources.ErrorTrackTelemetryException).FireAndForget();
            }
        }
    }
}
