using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinUIDesktopApp.Contracts.Services
{
    public interface INavigationService
    {
        event NavigatedEventHandler Navigated;

        bool CanGoBack { get; }

        Frame Frame { get; set; }

        bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false);

        void GoBack();
        
        void SetListDataItemForNextConnectedAnimation(object item);
    }
}
