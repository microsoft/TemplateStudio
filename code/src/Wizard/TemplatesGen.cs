using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.Resources;
using Microsoft.Templates.Wizard.Vs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard
{
    public class TemplatesGen
    {
        private TemplatesRepository _repository;
        private GenShell _shell;

        //TODO: ERROR HANDLING
        public TemplatesGen(GenShell shell) : this(shell, new TemplatesRepository(new CdnTemplatesLocation()))
        {

        }

        public TemplatesGen(GenShell shell, TemplatesRepository repository)
        {
            _shell = shell;
            _repository = repository;

            //TODO: MOVE THIS. IS FAILING BECAUSE IS LOCKING AN ICON FILE
            _repository.Sync();
        }

        public IEnumerable<GenInfo> GetUserSelection(WizardSteps selectionSteps)
        {
            var host = new WizardHost(selectionSteps, _repository, _shell);
            var result = host.ShowDialog();

            if (result.HasValue && result.Value)
            {
                return host.Result.ToArray();
            }
            else
            {
                _shell.CancelWizard();
            }
            return null;
        }

        public void Generate(IEnumerable<GenInfo> genItems)
        {
            if (genItems != null)
            {
                var outputPath = _shell.OutputPath;
                var outputs = new List<string>();

                foreach (var genInfo in genItems)
                {
                    if (genInfo.Template == null)
                    {
                        continue;
                    }

                    outputPath = GetOutputPath(outputs, outputPath);

                    //TODO: REVIEW ASYNC
                    var result = TemplateCreator.InstantiateAsync(genInfo.Template, genInfo.Name, null, outputPath, genInfo.Parameters, false).Result;

                    if (result.Status != CreationResultStatus.CreateSucceeded)
                    {
                        //TODO: THROW EXECPTION
                    }

                    if (result.PrimaryOutputs != null)
                    {
                        outputs.AddRange(result.PrimaryOutputs.Select(o => o.Path));
                    }

                    //TODO: EXECUTE POST ACTIONS
                    if (genInfo.Template.GetTemplateType() == TemplateType.Project)
                    {
                        //TODO: FILTER BY CSPROJ
                        foreach (var genOutput in result.PrimaryOutputs)
                        {
                            if (!string.IsNullOrWhiteSpace(genOutput.Path))
                            {
                                var projectPath = Path.GetFullPath(Path.Combine(outputPath, genOutput.Path));
                                _shell.AddProjectToSolution(projectPath);
                            }
                        }
                    }
                    else if (genInfo.Template.GetTemplateType() == TemplateType.Page)
                    {
                        foreach (var output in result.PrimaryOutputs)
                        {
                            if (!string.IsNullOrWhiteSpace(output.Path))
                            {
                                var itemPath = Path.GetFullPath(Path.Combine(outputPath, output.Path));
                                _shell.AddItemToActiveProject(itemPath);
                            }
                        }
                    }

                    //_shell.SaveSolution();
                }
            }
        }

        private static string GetOutputPath(IEnumerable<string> outputs, string outputPath)
        {
            if (outputs == null)
            {
                return outputPath;
            }
            var projectPath = outputs.FirstOrDefault(p => p.ToLower().EndsWith("proj"));

            if (string.IsNullOrEmpty(projectPath))
            {
                return outputPath;
            }

            return Path.GetDirectoryName(projectPath);
        }
    }
}
