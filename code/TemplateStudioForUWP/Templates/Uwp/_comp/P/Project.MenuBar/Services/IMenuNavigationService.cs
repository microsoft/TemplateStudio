using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Param_RootNamespace.Services
{
    public interface IMenuNavigationService
    {
        void Initialize(SplitView splitView, Frame rightFrame);

        void UpdateView(string pageToken, object parameters = null);

        void Navigate(string pageToken, object parameter = null);

        void OpenInRightPane(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null);

        void CloseRightPane();

        Task OpenInNewWindow(Type pageType);

        Task OpenInDialog(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null);
    }
}
