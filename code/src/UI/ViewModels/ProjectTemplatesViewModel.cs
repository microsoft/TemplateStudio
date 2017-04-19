using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.ViewModels
{
    public class ProjectTemplatesViewModel : Observable
    {
        private string _pagesHeader;
        public string PagesHeader
        {
            get { return _pagesHeader; }
            set { SetProperty(ref _pagesHeader, value); }
        }

        private string _featuresHeader;
        public string FeaturesHeader
        {
            get { return _featuresHeader; }
            set { SetProperty(ref _featuresHeader, value); }
        }

        public ObservableCollection<TemplateInfoViewModel> Pages { get; } = new ObservableCollection<TemplateInfoViewModel>();

        public async Task IniatializeAsync()
        {
        }
    }
}
