using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Wizard.Error;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.Resources;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Microsoft.Templates.Wizard
{
    public class GenController
    {
        static GenController()
        {
            //TODO: WHERE INITIALIZE THIS??
            AppHealth.Current.AddWriter(new ShellHealthWriter());
        }

        public static WizardState GetUserSelection(WizardSteps selectionSteps)
        {
            var host = new WizardHost(selectionSteps);

            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(host);

                if (host.Result != null)
                {
                    //TODO: Review when right-click-actions available to track Project or Page completed.
                    AppHealth.Current.Telemetry.TrackWizardCompletedAsync(WizardTypeEnum.NewProject).FireAndForget();

                    return host.Result;
                }
                else
                {
                    //TODO: Review when right-click-actions available to track Project or Page cancelled.
                    AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.NewProject).FireAndForget();
                }

            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                host.SafeClose();
                ShowError(ex, Resources.StringRes.ExceptionUnexpectedWizard);
            }
            GenContext.ToolBox.Shell.CancelWizard();
            return null;
        }

        public static void Generate(WizardState userSelection)
        {
            try
            {
                var genItems = GenComposer.Compose(userSelection).ToList();

                Stopwatch chrono = Stopwatch.StartNew();

                Dictionary<string, TemplateCreationResult> genResults = new Dictionary<string, TemplateCreationResult>();

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

                    AppHealth.Current.Verbose.TrackAsync($"Generating the template {genInfo.Template.Name} to {GenContext.Current.OutputPath}.").FireAndForget();

                    //TODO: REVIEW ASYNC
                    var result = CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, GenContext.Current.OutputPath, genInfo.Parameters, false, false).Result;

                    genResults.Add($"{genInfo.Template.Identity}_{genInfo.Name}", result);

                    if (result.Status != CreationResultStatus.Success)
                    {
                        throw new GenException(genInfo.Name, genInfo.Template.Name, result.Message);
                    }

                    ExecutePostActions(genInfo, result);
                }

                ExecuteGlobalPostActions(genItems);

                chrono.Stop();
                TrackTelemery(genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.Framework);
            }
            catch (Exception ex)
            {
                GenContext.ToolBox.Shell.CloseSolution();

                ShowError(ex, StringRes.ExceptionUnexpectedGenerating);

                GenContext.ToolBox.Shell.CancelWizard(false);
            }
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
            //Get post actions from template
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
                    return string.Format(StringRes.AddProjectMessage, genInfo.Name);
                case TemplateType.Page:
                    return string.Format(StringRes.AddPageMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                default:
                    return null;
            }
        }

        private static void ShowError(Exception ex, string textFormat)
        {
            AppHealth.Current.Error.TrackAsync(ex.ToString()).FireAndForget();
            AppHealth.Current.Exception.TrackAsync(ex).FireAndForget();

            var error = new ErrorDialog(ex);
            GenContext.ToolBox.Shell.ShowModal(error);
        }

        private static void CleanStatusBar()
        {
            GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);
        }

        private static void TrackTelemery(IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent, string appFx)
        {
            try
            {
                var pagesAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Count();
                //var featuresAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Count();

                foreach (var genInfo in genItems)
                {
                    if (genInfo.Template == null)
                    {
                        continue;
                    }

                    string resultsKey = $"{genInfo.Template.Identity}_{genInfo.Name}";
                    if (genInfo.Template.GetTemplateType() == TemplateType.Project)
                    {
                        AppHealth.Current.Telemetry.TrackProjectGenAsync(genInfo.Template, appFx, genResults[resultsKey], pagesAdded, null, timeSpent).FireAndForget();
                    }
                    else
                    {
                        AppHealth.Current.Telemetry.TrackPageOrFeatureTemplateGenAsync(genInfo.Template, appFx, genResults[resultsKey]).FireAndForget();
                    }
                }
            }
            catch (System.Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, "Exception tracking telemetry for Template Generation.").FireAndForget();
            }
        }
    }
}
