using EnvDTE;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Vs;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Extension
{
    public class SolutionWizard : IWizard
    {

        private Dictionary<string, string> replacements = new Dictionary<string, string>();

        GenerationWizard _genWizard;

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
            _genWizard.AddProjectFinish();
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            replacements = replacementsDictionary;

            SolutionInfo solutionInfo = new SolutionInfo(replacements);
            IVisualStudioShell vsShell = new VisualStudioShell();

            _genWizard = new GenerationWizard(vsShell, solutionInfo);

            if (runKind == WizardRunKind.AsNewProject || runKind == WizardRunKind.AsMultiProject)
            {
               _genWizard.AddProjectInit();
            }
        }

        public bool ShouldAddProjectItem(string filePath) => true;

    }
}
