using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Navigation;
using WinUI3App.Contracts.ViewModels;

namespace WinUI3App.ViewModels
{
    public class MainViewModel : ObservableRecipient, INavigationAware
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public MainViewModel()
        {
        }

        public void OnNavigatedTo(object parameter)
        {
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
