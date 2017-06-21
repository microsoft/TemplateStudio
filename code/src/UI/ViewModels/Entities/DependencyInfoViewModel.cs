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
using Microsoft.Templates.UI.Views;

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
            // InformationViewModel.Current.Initialize(_item);
            var infoView = new InformationWindow(_item, MainViewModel.Current.MainView);

            infoView.ShowDialog();
        }
    }
}
