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
                return host.Result.ToArray();
            }
            else
            {
                //TODO: Review when right-click-actions available to track Project or Page cancelled.
                AppHealth.Current.Telemetry.TrackCancellationAsync().FireAndForget();

                Shell.CancelWizard();
            }
            return null;
        }

        public void Generate(IEnumerable<GenInfo> genItems)
        {
            if (genItems != null)
            {
                var outputPath = Shell.OutputPath;
                var outputs = new List<string>();

                //TODO: RAULMGC record Telemetry for global project afterwards
                var pagesAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Count();
                var featuresAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Count();

                foreach (var genInfo in genItems)
                {
                    if (genInfo.Template == null)
                    {
                        continue;
                    }

                    outputPath = GetOutputPath(genInfo.Template);
                    AddSystemParams(genInfo);

                    //TODO: REVIEW ASYNC
                    var result = CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, outputPath, genInfo.Parameters, false).Result;

                    if (result.Status != CreationResultStatus.CreateSucceeded)
                    {
                        //TODO: THROW EXECPTION
                        AppHealth.Current.Telemetry.TrackTemplateGeneratedErrorAsync(genInfo.Template, pagesAdded, featuresAdded, result.Status, result.Message).FireAndForget();
                    }
                    else
                    {
                        AppHealth.Current.Telemetry.TrackTemplateGeneratedOkAsync(genInfo.Template, pagesAdded, featuresAdded).FireAndForget();
                    }

                    if (result.PrimaryOutputs != null)
                    {
                        outputs.AddRange(result.PrimaryOutputs.Select(o => o.Path));
                    }

					var postActionResults = ExecutePostActions(outputPath, genInfo, result);
                }
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
