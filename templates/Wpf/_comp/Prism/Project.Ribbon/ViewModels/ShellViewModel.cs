using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Param_RootNamespace.Constants;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private IRegionNavigationService _navigationService;
        private ICommand _loadedCommand;

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new DelegateCommand(OnLoaded));

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void OnLoaded()
        {
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
            _navigationService.RequestNavigate(PageKeys.Param_HomeName);
        }
    }
}
