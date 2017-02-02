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
        public GenInfo Info { get; set; }
        public PageViewModel(GenInfo genInfo)
        {
            //TODO: CHECK NULLS
            Info = genInfo;

            Name = genInfo.Name;
            TemplateName = genInfo.Template.Name;
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
    }
}
