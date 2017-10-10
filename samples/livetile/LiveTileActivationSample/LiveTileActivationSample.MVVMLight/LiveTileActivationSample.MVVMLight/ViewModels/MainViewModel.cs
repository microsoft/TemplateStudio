using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveTileActivationSample.MVVMLight.Services;
using Windows.UI.Xaml;

namespace LiveTileActivationSample.MVVMLight.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModels.ViewModelLocator Locator => Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

        private NavigationServiceEx NavigationService => Locator.NavigationService;

        private ICommand _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new RelayCommand(OnNavigate));

        public MainViewModel()
        {
        }

        private void OnNavigate()
        {
            NavigationService.Navigate(typeof(SecondarySectionViewModel).FullName);
        }
    }
}
