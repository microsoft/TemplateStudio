using EnvDTE;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Extension.Diagnostsics;
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
using System.Windows.Input;

namespace Microsoft.Templates.Extension
{
    public class SolutionWizard : IWizard
    {

        private Dictionary<string, string> replacements = new Dictionary<string, string>();
        private GenerationWizard _genWizard;

        public SolutionWizard()
        {
            AppHealth.Current.AddWriter(new VisualStudioOutputHealthWriter());
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
            Mouse.OverrideCursor = Cursors.Wait;
            try
            { 
                _genWizard.AddProjectFinish();
                AppHealth.Current.Verbose.TrackAsync("Generation finished").FireAndForget();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AppHealth.Current.Verbose.TrackAsync("Creating UWP Community Templates project...").FireAndForget();

                replacements = replacementsDictionary;

                SolutionInfo solutionInfo = new SolutionInfo(replacements);
                IVisualStudioShell vsShell = new VisualStudioShell();

                Mouse.OverrideCursor = null;

                _genWizard = new GenerationWizard(vsShell, solutionInfo);

                Mouse.OverrideCursor = Cursors.Wait;

                if (runKind == WizardRunKind.AsNewProject || runKind == WizardRunKind.AsMultiProject)
                {
                    _genWizard.AddProjectInit();
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        public bool ShouldAddProjectItem(string filePath) => true;

    }
}
