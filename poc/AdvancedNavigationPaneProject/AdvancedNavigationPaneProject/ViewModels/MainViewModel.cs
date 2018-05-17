using System.Windows.Input;
using AdvancedNavigationPaneProject.Helpers;
using AdvancedNavigationPaneProject.Services;
using AdvancedNavigationPaneProject.Views;

namespace AdvancedNavigationPaneProject.ViewModels
{
    public class MainViewModel : Observable
    {
        private ICommand _secondShellCommand;

        public ICommand SecondShellCommand => _secondShellCommand ?? (_secondShellCommand = new RelayCommand(OnSecondShell));

        public MainViewModel()
        {
        }

        private void OnSecondShell()
        {
            NavigationService.Navigate<SecondShellPage>(NavigationService.FrameKeyMain);
            NavigationService.Navigate<SecondMainPage>(NavigationService.FrameKeySecondary);
        }
    }
}
