using System;
using System.Windows.Input;

using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
using Param_RootNamespace.Views;

using Prism.Commands;
using Prism.Windows.Mvvm;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private IMenuNavigationService _menuNavigationService;

        public ICommand MenuFileExitCommand { get; }

        public ShellViewModel(IMenuNavigationService menuNavigationService)
        {
            _menuNavigationService = menuNavigationService;
            MenuFileExitCommand = new DelegateCommand(OnMenuFileExit);
        }

        public void Initialize(SplitView splitView, Frame rightFrame)
        {
            _menuNavigationService.Initialize(splitView, rightFrame);
        }

        private void OnMenuFileExit()
        {
            Application.Current.Exit();
        }
    }
}
