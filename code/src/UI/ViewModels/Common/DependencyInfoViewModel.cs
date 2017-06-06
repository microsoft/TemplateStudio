// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
            else if(_newItemTemplateItem != null)
            {
                NewItem.MainViewModel.Current.InfoShapeVisibility = System.Windows.Visibility.Visible;
                var infoView = new Views.NewProject.InformationWindow(_newItemTemplateItem, NewItem.MainViewModel.Current.MainView);
                infoView.ShowDialog();
                NewItem.MainViewModel.Current.InfoShapeVisibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
