using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Steps.SummaryStep
{
    public class TemplatesSummaryViewModel : ObservableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();
    }
}
