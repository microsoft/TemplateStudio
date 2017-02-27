using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Wizard.Dialog;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.Resources;
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
        private static GenComposer _composer;
        private static TemplatesRepository _repository;

        public GenShell Shell { get; }


        //TODO: ERROR HANDLING
        public GenController(GenShell shell) : this(shell, new TemplatesRepository(new RemoteTemplatesLocation()))
        {
        }

        public GenController(GenShell shell, TemplatesRepository repository)
        {
            Shell = shell;
            _repository = repository;

            _composer = new GenComposer(shell, repository);
        }

        public WizardState GetUserSelection(WizardSteps selectionSteps)
        {
            CleanStatusBar();

            var host = new WizardHost(selectionSteps, _repository, Shell);
            Shell.ShowModal(host);

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

                Shell.CancelWizard();
            }
            return null;
        }

        public void Generate(WizardState userSelection)
        {
            var genItems = _composer.Compose(userSelection).ToList();

            Stopwatch chrono = Stopwatch.StartNew();

            Dictionary<string, TemplateCreationResult> genResults = new Dictionary<string, TemplateCreationResult>();

            var outputPath = Shell.OutputPath;

            foreach (var genInfo in genItems)
            {
                if (genInfo.Template == null)
                {
                    continue;
                }

                var statusText = GetStatusText(genInfo);

                if (!string.IsNullOrEmpty(statusText))
                {
                    Shell.ShowStatusBarMessage(statusText);
                }

                outputPath = GetOutputPath(genInfo.Template);


                AppHealth.Current.Verbose.TrackAsync($"Generating the template {genInfo.Template.Name} to {outputPath}.").FireAndForget();

                //TODO: REVIEW ASYNC
                var result = CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, outputPath, genInfo.Parameters, false, false).Result;

                genResults.Add($"{genInfo.Template.Identity}_{genInfo.Name}", result);

                if (result.Status != CreationResultStatus.Success)
                {
                    //TODO: THROW EXCEPTION ?
                }

                ExecutePostActions(genInfo, result);
            }

            ExecuteGlobalPostActions(genItems);

            chrono.Stop();
            TrackTelemery(genItems, genResults, chrono.Elapsed.TotalSeconds);
        }

        private void ExecuteGlobalPostActions(List<GenInfo> genItems)
        {
            var postActions = PostActionFactory.FindGlobal(Shell, genItems);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        private static void TrackTelemery(IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent)
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
                    string appFx = genInfo.GetFramework();
                    if (string.IsNullOrEmpty(appFx))
                    {
                        // TODO: Review error tracking. Must this error reach the telemetry?
                        AppHealth.Current.Error.TrackAsync("Project framework does not found").FireAndForget();
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

        private string GetOutputPath(ITemplateInfo templateInfo)
        {
            if (templateInfo.GetTemplateType() == TemplateType.Project)
            {
                return Shell.OutputPath;
            }
            else
            {
                return Shell.ProjectPath;
            }
        }


        private void ExecutePostActions(GenInfo genInfo, TemplateCreationResult generationResult)
        {
            //Get post actions from template
            var postActions = PostActionFactory.Find(Shell, genInfo, generationResult);

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

        private void CleanStatusBar()
        {
            Shell.ShowStatusBarMessage(string.Empty);
        }
    }
}
