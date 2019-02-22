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
        public ICommand MenuFileExitCommand { get; }

        public ShellViewModel()
        {
            MenuFileExitCommand = new DelegateCommand(OnMenuFileExit);
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
