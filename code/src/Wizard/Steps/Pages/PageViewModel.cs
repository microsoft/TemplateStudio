using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Steps.Pages
{
    public class PageViewModel : ObservableBase
    {
        public PageViewModel(string name, string templateName, bool @readonly = false)
        {
            Name = name;
            TemplateName = templateName;
            Readonly = @readonly;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private string _templateName;
        public string TemplateName
        {
            get { return _templateName; }
            set
            {
                SetProperty(ref _templateName, value);
            }
        }

        private bool _readonly;
        public bool Readonly
        {
            get { return _readonly; }
            set
            {
                SetProperty(ref _readonly, value);
            }
        }
    }
}
