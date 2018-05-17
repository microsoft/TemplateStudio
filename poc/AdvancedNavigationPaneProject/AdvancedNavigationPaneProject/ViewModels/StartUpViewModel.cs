using System;
using System.Windows.Input;
using AdvancedNavigationPaneProject.Helpers;
using AdvancedNavigationPaneProject.Services;
using AdvancedNavigationPaneProject.Views;

namespace AdvancedNavigationPaneProject.ViewModels
{
    public class StartUpViewModel : Observable
    {
        private ICommand _startUpCommand;

        public ICommand StartUpCommand => _startUpCommand ?? (_startUpCommand = new RelayCommand(OnStartUp));

        public StartUpViewModel()
        {
        }

        private void OnStartUp()
        {
            NavigationService.Navigate<ShellPage>(NavigationService.FrameKeyMain, new NavigationConfig(registerOnBackStack: false));
            NavigationService.Navigate<MainPage>(NavigationService.FrameKeySecondary);
        }
    }
}
