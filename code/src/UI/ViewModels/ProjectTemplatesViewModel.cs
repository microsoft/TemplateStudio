using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private RelayCommand<TemplateInfoViewModel> _removeItemCommand;
        public RelayCommand<TemplateInfoViewModel> RemoveItemCommand => _removeItemCommand ?? (_removeItemCommand = new RelayCommand<TemplateInfoViewModel>(RemoveItem));
        private void RemoveItem(TemplateInfoViewModel item)
        {
            if (Pages.Contains(item))
            {
                Pages.Remove(item);
            }
            else if (Features.Contains(item))
            {
                Features.Remove(item);
            }
        }

        public ObservableCollection<TemplateInfoViewModel> Pages { get; } = new ObservableCollection<TemplateInfoViewModel>();
        public ObservableCollection<TemplateInfoViewModel> Features { get; } = new ObservableCollection<TemplateInfoViewModel>();

        public async Task IniatializeAsync()
        {
        }
    }
}
