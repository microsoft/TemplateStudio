// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;

using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.Common
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

        private NewProject.TemplateInfoViewModel _newProjectTemplateItem;

        private NewItem.TemplateInfoViewModel _newItemTemplateItem;

        public DependencyInfoViewModel(NewProject.TemplateInfoViewModel item)
        {
            _newProjectTemplateItem = item;
            Name = item.Name;
        }

        public DependencyInfoViewModel(NewItem.TemplateInfoViewModel item)
        {
            _newItemTemplateItem = item;
            Name = item.Name;
        }

        private void OnItemClick()
        {
            if (_newProjectTemplateItem != null)
            {
                var infoView = new Views.NewProject.InformationWindow(_newProjectTemplateItem, NewProject.MainViewModel.Current.MainView);
                infoView.ShowDialog();
            }
            else if (_newItemTemplateItem != null)
            {
                NewItem.MainViewModel.Current.WizardStatus.InfoShapeVisibility = System.Windows.Visibility.Visible;
                var infoView = new Views.NewProject.InformationWindow(_newItemTemplateItem, NewItem.MainViewModel.Current.MainView);
                infoView.ShowDialog();
                NewItem.MainViewModel.Current.WizardStatus.InfoShapeVisibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
