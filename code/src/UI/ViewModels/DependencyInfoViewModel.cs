using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Mvvm;
using System.Windows.Input;
using System;

namespace Microsoft.Templates.UI.ViewModels
{
    public class DependencyInfoViewModel : Observable
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ICommand _showInfoCommand;
        public ICommand ShowInfoCommand => _showInfoCommand ?? (_showInfoCommand = new RelayCommand(OnItemClick));

        private TemplateInfoViewModel _item;
        public DependencyInfoViewModel(TemplateInfoViewModel item)
        {
            _item = item;
            Name = item.Name;            
        }

        private void OnItemClick()
        {
            InformationViewModel.Current.Initialize(_item);
        }
    }
}
