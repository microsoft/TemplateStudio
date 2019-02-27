using System.Windows.Input;

using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
using Param_RootNamespace.Views;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : Observable
    {
        private ICommand _menuFileExitCommand;

        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

        public ShellViewModel()
        {
        }

        public void Initialize(Frame shellFrame, SplitView splitView, Frame rightFrame)
        {
            NavigationService.Frame = shellFrame;
            MenuNavigationHelper.Initialize(splitView, rightFrame);
        }

        private void OnMenuFileExit()
        {
            Application.Current.Exit();
        }
    }
}
