using System;
using System.Windows.Input;
using LiveTileActivationSample.MVVMBasic.Helpers;
using LiveTileActivationSample.MVVMBasic.Services;
using LiveTileActivationSample.MVVMBasic.Views;

namespace LiveTileActivationSample.MVVMBasic.ViewModels
{
    public class MainViewModel : Observable
    {
        private ICommand _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new RelayCommand(OnNavigate));

        public MainViewModel()
        {
        }

        private void OnNavigate()
        {
            NavigationService.Navigate<SecondarySectionPage>();
        }
    }
}
