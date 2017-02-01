using EnvDTE;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Extension.Diagnostsics;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Host;
using Microsoft.VisualStudio.TemplateWizard;
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
        private TemplatesGen _gen;
        private IEnumerable<GenInfo> _selectedTemplates;

        public SolutionWizard()
        {
            AppHealth.Current.AddWriter(new VsOutputHealthWriter());
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
            AppHealth.Current.Verbose.TrackAsync("Creating UWP Community Templates project...").FireAndForget();
            _gen.Generate(_selectedTemplates);
            AppHealth.Current.Verbose.TrackAsync("Generation finished").FireAndForget();

            _gen.Shell.ShowTaskList();
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

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
