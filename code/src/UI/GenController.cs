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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.UI.Views.Common;
using Microsoft.Templates.UI.Resources;
using Microsoft.VisualStudio.TemplateWizard;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using System.Xml.Linq;


namespace Microsoft.Templates.UI
{
    public class GenController
    {
        public static (string ProjectType, string Framework) ReadProjectConfiguration()
        {
            //TODO: Review this
            var path = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
            if (File.Exists(path))
            {
                var manifest = XElement.Load(path);

                var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == "Metadata");
                var projectType = metadata?.Descendants().FirstOrDefault(m => m.Attribute("Name").Value == "projectType")?.Attribute("Value")?.Value;
                var framework = metadata?.Descendants().FirstOrDefault(m => m.Attribute("Name").Value == "framework")?.Attribute("Value")?.Value;

                return (projectType, framework);
            }
            
            return (string.Empty, string.Empty);
        }

        public static UserSelection GetUserSelection()
        {
            var mainView = new Views.NewProject.MainView();

            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(mainView);
                if (mainView.Result != null)
                {
                    //TODO: Review when right-click-actions available to track Project or Page completed.
                    AppHealth.Current.Telemetry.TrackWizardCompletedAsync(WizardTypeEnum.NewProject).FireAndForget();

                    return mainView.Result;
                }
                else
                {
                    //TODO: Review when right-click-actions available to track Project or Page cancelled.
                    AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.NewProject).FireAndForget();
                }

            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                mainView.SafeClose();
                ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return null;
        }

        public static UserSelection GetUserSelectionNewItem(TemplateType templateType)
        {
            //var newItem = new Views.NewItem.NewItemView();
            var newItem = new Views.NewItem.MainView(templateType);

            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(newItem);
                if (newItem.Result != null)
                {
                    //TODO: Review when right-click-actions available to track Project or Page completed.
                    //AppHealth.Current.Telemetry.TrackWizardCompletedAsync(WizardTypeEnum.NewItem).FireAndForget();

                    return newItem.Result;
                }
                else
                {
                    //TODO: Review when right-click-actions available to track Project or Page cancelled.
                    //AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.NewItem).FireAndForget();
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

        public static async Task GenerateProjectAsync(UserSelection userSelection)
        {
            try
            {
                await UnsafeGenerateProjectAsync(userSelection);
            }
            catch (Exception ex)
            {
                GenContext.ToolBox.Shell.CloseSolution();

                ShowError(ex, userSelection);

                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public static async Task GenerateNewItemAsync(UserSelection userSelection)
        {
            try
            {
               await UnsafeGenerateNewItemAsync(userSelection);
            }
            catch (Exception ex)
            {
                

                ShowError(ex, userSelection);

                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public static async Task UnsafeGenerateProjectAsync(UserSelection userSelection)
        {
            var genItems = GenComposer.Compose(userSelection).ToList();
            var chrono = Stopwatch.StartNew();

            var genResults = new Dictionary<string, TemplateCreationResult>();

            foreach (var genInfo in genItems)
            {
                if (genInfo.Template == null)
                {
                    continue;
                }

                var statusText = GetStatusText(genInfo);

                if (!string.IsNullOrEmpty(statusText))
                {
                    GenContext.ToolBox.Shell.ShowStatusBarMessage(statusText);
                }

                AppHealth.Current.Info.TrackAsync($"Generating the template {genInfo.Template.Name} to {GenContext.Current.OutputPath}.").FireAndForget();

                var result = await CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, GenContext.Current.OutputPath, genInfo.Parameters, false, false, null);

                genResults.Add($"{genInfo.Template.Identity}_{genInfo.Name}", result);

                if (result.Status != CreationResultStatus.Success)
                {
                    throw new GenException(genInfo.Name, genInfo.Template.Name, result.Message);
                }

                ExecuteNewProjectPostActions(genInfo, result);
            }

            ExecuteGlobalNewProjectPostActions();

            chrono.Stop();

            TrackTelemery(genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.ProjectType, userSelection.Framework);
        }

        public static async Task UnsafeGenerateNewItemAsync(UserSelection userSelection)
        {
            var genItems = GenComposer.ComposeNewItem(userSelection).ToList();
            var chrono = Stopwatch.StartNew();

            var genResults = new Dictionary<string, TemplateCreationResult>();

            foreach (var genInfo in genItems)
            {
                if (genInfo.Template == null)
                {
                    continue;
                }

                var statusText = GetStatusText(genInfo);

                if (!string.IsNullOrEmpty(statusText))
                {
                    GenContext.ToolBox.Shell.ShowStatusBarMessage(statusText);
                }

                AppHealth.Current.Info.TrackAsync($"Generating the template {genInfo.Template.Name} to {GenContext.Current.OutputPath}.").FireAndForget();

                var result = await CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, GenContext.Current.OutputPath, genInfo.Parameters, false, false, null);

                genResults.Add($"{genInfo.Template.Identity}_{genInfo.Name}", result);

                if (result.Status != CreationResultStatus.Success)
                {
                    throw new GenException(genInfo.Name, genInfo.Template.Name, result.Message);
                }

                ExecuteNewItemPostActions(genInfo, result);
            }

            ExecuteGlobalNewItemPostActions();

            chrono.Stop();

            // TODO: Review New Item telemetry
            TrackTelemery(genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.ProjectType, userSelection.Framework);
        }

        public static bool GetUserSyncDescision()
        {
  
            var result = CompareOutputAndProject();
            
            var syncNewItemView = new Views.NewItem.SyncNewItemView();

            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(syncNewItemView);

                if (syncNewItemView.Result)
                {
                    //TODO: Review when right-click-actions available to track Project or Page completed.
                    //AppHealth.Current.Telemetry.TrackWizardCompletedAsync(WizardTypeEnum.NewProject).FireAndForget();  
                }
                else
                {
                    //TODO: Review when right-click-actions available to track Project or Page cancelled.
                    //AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.NewProject).FireAndForget();
                }
                return syncNewItemView.Result;

            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                syncNewItemView.SafeClose();
                ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return false;
        }

        public static CompareResult CompareOutputAndProject()
        {
            var result = new CompareResult();
            var files = Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*", SearchOption.AllDirectories)
                .Where(f => !Regex.IsMatch(f, MergePostAction.PostactionRegex) && !Regex.IsMatch(f, MergePostAction.FailedPostactionRegex))
                .ToList();

            foreach (var file in files)
            {
                var destFilePath = file.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath);
                if (!File.Exists(destFilePath))
                {
                    result.NewFiles.Add(file.Replace(GenContext.Current.OutputPath, String.Empty));
                }
                else
                {
                    if (GenContext.Current.MergeFilesFromProject.Contains(destFilePath))
                    {
                        if (!FilesAreEqual(file, destFilePath))
                        {
                            result.ModifiedFiles.Add(destFilePath.Replace(GenContext.Current.ProjectPath, String.Empty));
                        }
                    }
                    else
                    {
                        if (!FilesAreEqual(file, destFilePath))
                        {
                            result.ConflictingFiles.Add(destFilePath.Replace(GenContext.Current.ProjectPath, String.Empty));
                        }
                    }
                }
            }

            return result;
        }

        public static void SyncNewItem(UserSelection userSelection)
        {
            try
            {
                UnsafeSyncNewItem();
                CleanupTempGeneration();
            }
            catch (Exception ex)
            {

                ShowError(ex, userSelection);

                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public static void UnsafeSyncNewItem()
        {
            var files = Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*", SearchOption.AllDirectories)
                .Where(f => !Regex.IsMatch(f, MergePostAction.PostactionRegex))
                .ToList();

            foreach (var file in files)
            {
                var destFileName = file.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath);
                var destDirectory = Path.GetDirectoryName(destFileName);
                if (!Directory.Exists(destDirectory))
                {
                    Directory.CreateDirectory(destDirectory);
                }
                File.Copy(file, destFileName, true);
            }

            ExecuteFinishItemGenerationPostActions();

        }

        public static void CleanupTempGeneration()
        {
            GenContext.Current.GenerationWarnings.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
            GenContext.Current.ProjectItems.Clear();
            var directory = GenContext.Current.OutputPath;
            try
            {             
                if (directory.Contains(Path.GetTempPath()))
                {
                    if (Directory.Exists(directory))
                    {
                        Directory.Delete(directory, true);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = $"The folder {directory} can't be delete. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
            }
        }


        private static bool FilesAreEqual(string file, string destFilePath)
        {
            return File.ReadAllBytes(file).SequenceEqual(File.ReadAllBytes(destFilePath));
        }

        private static void ExecuteGlobalNewProjectPostActions()
        {
            var postActions = PostActionFactory.FindGlobalNewProjectPostActions();

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void ExecuteGlobalNewItemPostActions()
        {
            var postActions = PostActionFactory.FindGlobalNewItemPostActions();

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void ExecuteFinishItemGenerationPostActions()
        {
            var postActions = PostActionFactory.FindFinishItemGenerationPostActions();

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void ExecuteNewProjectPostActions(GenInfo genInfo, TemplateCreationResult generationResult)
        {
            //Get post actions from template
            var postActions = PostActionFactory.FindNewProjectPostActions(genInfo, generationResult);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void ExecuteNewItemPostActions(GenInfo genInfo, TemplateCreationResult generationResult)
        {
            //Get post actions from template
            var postActions = PostActionFactory.FindNewItemPostActions(genInfo, generationResult);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static string GetStatusText(GenInfo genInfo)
        {
            switch (genInfo.Template.GetTemplateType())
            {
                case TemplateType.Project:
                    return string.Format(StringRes.GeneratingProjectMessage, genInfo.Name);
                case TemplateType.Page:
                    return string.Format(StringRes.GeneratingPageMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                case TemplateType.Feature:
                    return string.Format(StringRes.GeneratingFeatureMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                default:
                    return null;
            }
        }

        private static void ShowError(Exception ex, UserSelection userSelection = null)
        {
            AppHealth.Current.Error.TrackAsync(ex.ToString()).FireAndForget();
            AppHealth.Current.Exception.TrackAsync(ex, userSelection?.ToString()).FireAndForget();

            var error = new ErrorDialog(ex);

            GenContext.ToolBox.Shell.ShowModal(error);
        }

        private static void CleanStatusBar()
        {
            GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);
        }

        private static void TrackTelemery(IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent, string appProjectType, string appFx)
        {
            try
            {
                int pagesAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Count();
                int featuresAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Count();

                foreach (var genInfo in genItems)
                {
                    if (genInfo.Template == null)
                    {
                        continue;
                    }

                    string resultsKey = $"{genInfo.Template.Identity}_{genInfo.Name}";

                    if (genInfo.Template.GetTemplateType() == TemplateType.Project)
                    {
                        AppHealth.Current.Telemetry.TrackProjectGenAsync(genInfo.Template, 
                            appProjectType, appFx, genResults[resultsKey], pagesAdded, featuresAdded, timeSpent).FireAndForget();
                    }
                    else
                    {
                        AppHealth.Current.Telemetry.TrackItemGenAsync(genInfo.Template, appProjectType, appFx, genResults[resultsKey]).FireAndForget();
                    }
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, "Exception tracking telemetry for Template Generation.").FireAndForget();
            }
        }
    }
}
