using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardState
    {
        public string ProjectType { get; set; }
        public string Framework { get; set; }
        public List<(string name, string templateName)> Pages { get; } = new List<(string name, string templateName)>();
    }

    public class WizardContext : ObservableBase
    {
        public TemplatesRepository TemplatesRepository { get; }
        public GenShell Shell { get; }
        public WizardState State { get; } = new WizardState();


        public WizardContext(TemplatesRepository templatesRepository, GenShell shell)
        {
            TemplatesRepository = templatesRepository;
            Shell = shell;
        }

        private bool _canGoForward;
        public bool CanGoForward
        {
            get => _canGoForward;
            set => SetProperty(ref _canGoForward, value);
        }

        public IEnumerable<GenInfo> GetSelection()
        {
            var selection = new List<GenInfo>();

            
            return selection;
        }
    }
}
