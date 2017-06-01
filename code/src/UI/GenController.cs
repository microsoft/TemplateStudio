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
using Microsoft.Templates.UI.Views;
using Microsoft.Templates.UI.Resources;
using Microsoft.VisualStudio.TemplateWizard;
using System.IO;

namespace Microsoft.Templates.UI
{
    public class GenController
    {
        public static UserSelection GetUserSelection()
        {
            var mainView = new MainView();

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

        public static UserSelection GetUserSelectionNewItem()
        {
            var newItem = new NewItemView();

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
                GenContext.ToolBox.Shell.CloseSolution();

                ShowError(ex, userSelection);

                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public static void SyncNewItem(UserSelection userSelection)
        {
            try
            {
                UnsafeSyncNewItem();
            }
            catch (Exception ex)
            {
                GenContext.ToolBox.Shell.CloseSolution();

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

                ExecutePostActions(genInfo, result, GenContext.Current.GenerationMode);
            }

            ExecuteGlobalPostActions();

            ExecuteFinishGenerationPostActions(GenContext.Current.GenerationMode);

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

                ExecutePostActions(genInfo, result, GenContext.Current.GenerationMode);
            }

            ExecuteGlobalPostActions();

            chrono.Stop();

            // TODO: Review New Item telemetry
            TrackTelemery(genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.ProjectType, userSelection.Framework);
        }

        public static void UnsafeSyncNewItem()
        {
            foreach (var file in Directory.EnumerateFiles(GenContext.Current.OutputPath, "*", SearchOption.AllDirectories))
            {
                var destFileName = file.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath);
                File.Copy(file, destFileName, true);
            }

            ExecuteFinishGenerationPostActions(GenContext.Current.GenerationMode);

            Directory.Delete(GenContext.Current.OutputPath, true);

        }

        private static void ExecuteGlobalPostActions()
        {
            var postActions = PostActionFactory.FindGlobal();

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void ExecuteFinishGenerationPostActions(GenerationMode generationMode)
        {
            var postActions = PostActionFactory.FindFinishGenerationPostActions(generationMode);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void ExecutePostActions(GenInfo genInfo, TemplateCreationResult generationResult, GenerationMode generationMode)
        {
            //Get post actions from template
            var postActions = PostActionFactory.Find(genInfo, generationResult, generationMode);

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
