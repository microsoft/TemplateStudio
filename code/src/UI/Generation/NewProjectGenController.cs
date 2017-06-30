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
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.UI
{
    public class NewProjectGenController : GenController
    {
        private static Lazy<NewProjectGenController> _instance = new Lazy<NewProjectGenController>(Initialize);
        public static NewProjectGenController Instance => _instance.Value;

        private static NewProjectGenController Initialize()
        {
            return new NewProjectGenController(new NewProjectPostActionFactory());
        }

        private NewProjectGenController(PostActionFactory postactionFactory)
        {
            _postactionFactory = postactionFactory;
        }

        public static UserSelection GetUserSelection(string language)
        {
            var mainView = new Views.NewProject.MainView(language);

            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(mainView);
                if (mainView.Result != null)
                {
                    AppHealth.Current.Telemetry.TrackWizardCompletedAsync(WizardTypeEnum.NewProject).FireAndForget();

                    return mainView.Result;
                }
                else
                {
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

        public async Task GenerateProjectAsync(UserSelection userSelection)
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

        public async Task UnsafeGenerateProjectAsync(UserSelection userSelection)
        {
            var genItems = GenComposer.Compose(userSelection).ToList();
            var chrono = Stopwatch.StartNew();

            var genResults = await GenerateItemsAsync(genItems);

            chrono.Stop();

            TrackTelemery(genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.ProjectType, userSelection.Framework, userSelection.Language);
        }

        private static void ExecuteGlobalPostActions(List<GenInfo> genItems)
        {
            var postActions = PostActionFactory.FindGlobal(genItems);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void ExecutePostActions(GenInfo genInfo, TemplateCreationResult generationResult)
        {
            // Get post actions from template
            var postActions = PostActionFactory.Find(genInfo, generationResult);

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

        private static void TrackTelemery(IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent, string appProjectType, string appFx, string language)
        {
            try
            {
                int pagesAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Count();
                int featuresAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Count();
                var pageIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Select(t => t.Template.Identity));
                var featureIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Select(t => t.Template.Identity));

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

                            appProjectType, appFx, genResults[resultsKey], GenContext.ToolBox.Shell.GetVsProjectId(), language, pagesAdded, featuresAdded, pageIdentities, featureIdentities, timeSpent).FireAndForget();
                    }
                    else
                    {
                        AppHealth.Current.Telemetry.TrackItemGenAsync(genInfo.Template, GenSourceEnum.NewProject, appProjectType, appFx, genResults[resultsKey]).FireAndForget();
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
