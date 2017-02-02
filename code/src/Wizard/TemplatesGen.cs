using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Wizard.Dialog;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.PostActions;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Microsoft.Templates.Wizard
{
    public class TemplatesGen
    {
        private TemplatesRepository _repository;
        public GenShell Shell { get; }

        //TODO: ERROR HANDLING
        public TemplatesGen(GenShell shell) : this(shell, new TemplatesRepository(new RemoteTemplatesLocation()))
        {
        }

        public TemplatesGen(GenShell shell, TemplatesRepository repository)
        {
            Shell = shell;
            _repository = repository;
        }

        public IEnumerable<GenInfo> GetUserSelection(WizardSteps selectionSteps)
        {
            var host = new WizardHost(selectionSteps, _repository, Shell);
            var result = host.ShowDialog();

            if (result.HasValue && result.Value)
            {
                //TODO: Review when right-click-actions available to track Project or Page completed.
                AppHealth.Current.Telemetry.TrackWizardCompletedAsync(WizardTypeEnum.NewProject).FireAndForget();

                return host.Result.ToArray();
            }
            else
            {
                //TODO: Review when right-click-actions available to track Project or Page cancelled.
                AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.NewProject).FireAndForget();

                Shell.CancelWizard();
            }
            return null;
        }

        public void Generate(IEnumerable<GenInfo> genItems)
        {
            if (genItems != null)
            {
                Stopwatch chrono = Stopwatch.StartNew();

                Dictionary<string, TemplateCreationResult> genResults = new Dictionary<string, TemplateCreationResult>();

                var outputPath = Shell.OutputPath;
                var outputs = new List<string>();

                foreach (var genInfo in genItems)
                {
                    if (genInfo.Template == null)
                    {
                        continue;
                    }
                    
                    outputPath = GetOutputPath(genInfo.Template);
                    AddSystemParams(genInfo);

                    AppHealth.Current.Verbose.TrackAsync($"Generating the template {genInfo.Template.Name} to {outputPath}.").FireAndForget();

                    //TODO: REVIEW ASYNC
                    var result = CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, outputPath, genInfo.Parameters, false).Result;
                    genResults.Add(genInfo.Template.Identity, result);

                    if (result.Status != CreationResultStatus.CreateSucceeded)
                    {
                        //TODO: THROW EXCEPTION ?
                    }

                    if (result.PrimaryOutputs != null)
                    {
                        outputs.AddRange(result.PrimaryOutputs.Select(o => o.Path));
                    }

                    var postActionResults = ExecutePostActions(outputPath, genInfo, result);

                    chrono.Stop();

                    Shell.ShowTaskList();
                }

                var timeSpent = chrono.Elapsed.TotalSeconds;
                TrackTelemery(genItems, genResults, timeSpent);
            }
        }

        private static void TrackTelemery(IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent)
        {
            try
            {
                var pagesAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Count();
                var featuresAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Count();

                foreach (var genInfo in genItems)
                {
                    if (genInfo.Template == null)
                    {
                        continue;
                    }
                    if (genInfo.Template.GetTemplateType() == TemplateType.Project)
                    {
                        AppHealth.Current.Telemetry.TrackProjectGenAsync(genInfo.Template, genResults[genInfo.Template.Identity], pagesAdded, featuresAdded, timeSpent).FireAndForget();
                    }
                    else
                    {
                        AppHealth.Current.Telemetry.TrackPageOrFeatureTemplateGenAsync(genInfo.Template, genResults[genInfo.Template.Identity]).FireAndForget();
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

        private void AddSystemParams(GenInfo genInfo)
        {
            if (genInfo.Template.GetTemplateType() == TemplateType.Page)
            {
                genInfo.Parameters.Add("PageNamespace", Shell.GetActiveNamespace());
            }
        }

        private IEnumerable<PostActionResult> ExecutePostActions(string outputPath, GenInfo genInfo, TemplateCreationResult generationResult)
        {
            //Get post actions from template
            var postActions = PostActionCreator.GetPostActions(genInfo.Template);

            //Execute post action
            var postActionResults = new List<PostActionResult>();

            foreach (var postAction in postActions)
            {
                var postActionResult = postAction.Execute(outputPath, genInfo, generationResult, Shell);
                postActionResults.Add(postActionResult);
            }

            return postActionResults;
        }

        private static void ShowPostActionResults(IEnumerable<PostActionResult> postActionResults)
        {
            //TODO: Determine where to show postActionResults

            var postActionResultMessages = postActionResults.Aggregate(new StringBuilder(), (sb, a) => sb.AppendLine($"{a.Message}"), sb => sb.ToString());

            if (postActionResults.Any(p => p.ResultCode != ResultCode.Success))
            {
                var errorMessages = postActionResults
                                            .Where(p => p.ResultCode != ResultCode.Success)
                                            .Aggregate(new StringBuilder(), (sb, p) => sb.AppendLine($"{p.Message}"), sb => sb.ToString());

                //TODO: REVIEW THIS
                ErrorMessageDialog.Show("Some PostActions failed", "Failed post actions", errorMessages, MessageBoxImage.Error);
            }

            
        }
    }
}
