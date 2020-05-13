using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Param_RootNamespace.Constants;
using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IRightPaneService _rightPaneService;
        private IRegionNavigationService _navigationService;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new DelegateCommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new DelegateCommand(OnUnloaded));

        public ShellViewModel(IRegionManager regionManager, IRightPaneService rightPaneService)
        {
            _regionManager = regionManager;
            _rightPaneService = rightPaneService;
        }

        private void OnLoaded()
        {
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
            _navigationService.RequestNavigate(PageKeys.Param_HomeName);
        }

        private void OnUnloaded()
        {
            _regionManager.Regions.Remove(Regions.Main);
            _rightPaneService.CleanUp();
        }
    }
}
