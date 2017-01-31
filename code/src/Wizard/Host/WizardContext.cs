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
    public delegate void SaveStateEventHandler(object sender, EventArgs e);

    //TODO: REVIEW THIS NAME
    public class WizardContext : ObservableBase
    {
        public TemplatesRepository TemplatesRepository { get; }
        public GenShell Shell { get; }

        public event SaveStateEventHandler SaveState;

        //TODO: MAKE READONLY??
        public Dictionary<Type, GenInfo[]> SelectedTemplates { get; } = new Dictionary<Type, GenInfo[]>();
        public ProjectInfoViewModel SelectedProjectType { get; set; }
        public ProjectInfoViewModel SelectedFrameworkType { get; set; }

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

        public void NotifySave() => SaveState?.Invoke(this, new EventArgs());
    }
}
