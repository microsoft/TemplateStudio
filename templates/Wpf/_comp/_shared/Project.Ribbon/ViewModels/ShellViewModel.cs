using System.Windows.Input;
using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly IRightPaneService _rightPaneService;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new System.Windows.Input.ICommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new System.Windows.Input.ICommand(OnUnloaded));

        public ShellViewModel(IRightPaneService rightPaneService)
        {
            _rightPaneService = rightPaneService;
        }

        private void OnLoaded()
        {
        }

        private void OnUnloaded()
        {
            _rightPaneService.CleanUp();
        }
    }
}
