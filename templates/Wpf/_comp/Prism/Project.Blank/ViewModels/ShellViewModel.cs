using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Models;
using Param_RootNamespace.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly AppConfig _config;
        private readonly IRegionManager _regionManager;
        private IRegionNavigationService _navigationService;
        private DelegateCommand _goBackCommand;
        private ICommand _loadedCommand;

        public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new DelegateCommand(OnLoaded));

        public ShellViewModel(AppConfig config, IRegionManager regionManager)
        {
            _config = config;
            _regionManager = regionManager;
        }

        private void OnLoaded()
        {
            _navigationService = _regionManager.Regions[_config.MainRegion].NavigationService;
            _navigationService.RequestNavigate("Param_HomeName");
        }

        private bool CanGoBack()
            => _navigationService != null && _navigationService.Journal.CanGoBack;

        private void OnGoBack()
            => _navigationService.Journal.GoBack();
    }
}
