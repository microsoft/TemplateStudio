using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SplitViewTemplate.Services
{
    public interface INavigationService
    {
        void SetNavigationFrame(Frame frame);

        bool CanGoBack { get; }
        bool CanGoForward { get; }

        void GoBack();
        void GoForward();

        bool Navigate(string pageKey);
        bool Navigate(string pageKey, object parameter);
        bool Navigate(string pageKey, object parameter, NavigationTransitionInfo infoOverride);        
    }
}
