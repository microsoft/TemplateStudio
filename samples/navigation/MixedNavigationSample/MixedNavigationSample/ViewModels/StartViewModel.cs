using System;

using MixedNavigationSample.Helpers;
using System.Windows.Input;
using MixedNavigationSample.Services;
using Windows.UI.Xaml;

namespace MixedNavigationSample.ViewModels
{
    public class StartViewModel : Observable
    {
        public ICommand StartCommand { get; set; }

        public StartViewModel()
        {
            StartCommand = new RelayCommand(OnStart);    
        }

        private void OnStart()
        {
            NavigationService.Navigate<Views.ShellPage>();
            NavigationService.Navigate<Views.HomePage>();
        }
    }
}
