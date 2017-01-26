using EnvDTE;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Host;
using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;

namespace Microsoft.Templates.Extension
{
    public class SolutionWizard : IWizard
    {
        private TemplatesGen _gen;
        private IEnumerable<GenInfo> _selectedTemplates;

        public SolutionWizard()
        {
        }


        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
            _gen.Generate(_selectedTemplates);
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            var shell = new VsGenShell(replacementsDictionary);

            _gen = new TemplatesGen(shell);

            if (runKind == WizardRunKind.AsNewProject || runKind == WizardRunKind.AsMultiProject)
            {
                _selectedTemplates = _gen.GetUserSelection(WizardSteps.Project);
            }
        }

        public bool ShouldAddProjectItem(string filePath) => true;

    }
}
