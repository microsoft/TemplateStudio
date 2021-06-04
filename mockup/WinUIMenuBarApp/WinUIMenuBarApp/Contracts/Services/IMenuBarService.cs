using Microsoft.UI.Xaml.Controls;

namespace WinUIMenuBarApp.Contracts.Services
{
    public interface IMenuBarService
    {
        void Initialize(SplitView splitView, Frame rightFrame);

        void UpdateView(string pageKey, object parameter = null);

        void NavigateTo(string pageKey, object parameter = null);

        void OpenInRightPane(string fullName, object parameter = null);

        void Exit();
    }
}
