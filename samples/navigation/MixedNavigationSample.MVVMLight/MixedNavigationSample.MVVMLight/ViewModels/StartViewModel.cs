using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using MixedNavigationSample.MVVMLight.Services;

using System.Windows.Input;

using Windows.UI.Xaml;


namespace MixedNavigationSample.MVVMLight.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        private ViewModelLocator Locator => Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;
        private NavigationServiceEx NavigationService => Locator.NavigationService;
        public ICommand StartCommand { get; set; }

        public StartViewModel()
        {
            StartCommand = new RelayCommand(OnStart);
        }

        private void OnStart()
        {
            //Set Window.Current.Content to a new ShellPage, this will replace NavigationService frame for an inner frame to change navigation handling.
            Window.Current.Content = new Views.ShellPage();

            //Navigating now to a HomePage, this will be the first navigation on a NavigationPane menu
            NavigationService.Navigate(typeof(ViewModels.HomeViewModel).FullName);
        }
    }
}
