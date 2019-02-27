using System;
using System.Threading.Tasks;
using Param_RootNamespace.Views;
using Prism.Windows.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Param_RootNamespace.Services
{
    public class MenuNavigationService : IMenuNavigationService
    {
        private object _lastParamUsed;
        private SplitView _splitView;
        private Frame _rightFrame;
        private INavigationService _navigationServiceInstance;

        public MenuNavigationService(INavigationService navigationServiceInstance)
        {
            _navigationServiceInstance = navigationServiceInstance;
        }

        public void Initialize(SplitView splitView, Frame rightFrame)
        {
            _splitView = splitView;
            _rightFrame = rightFrame;
        }

        public void UpdateView(string pageToken, object parameters = null)
        {
            _navigationServiceInstance.Navigate(pageToken, parameters);
            _navigationServiceInstance.RemoveLastPage();
        }

        public void Navigate(string pageToken, object parameter = null)
        {
            _navigationServiceInstance.Navigate(pageToken, parameter);
        }

        public void OpenInRightPane(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            // Don't open the same page multiple times
            if (_rightFrame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParamUsed)))
            {
                var navigationResult = _rightFrame.Navigate(pageType, parameter, infoOverride);
                if (navigationResult)
                {
                    _lastParamUsed = parameter;
                }
            }

            _splitView.IsPaneOpen = true;
        }

        public async Task OpenInNewWindow(Type pageType)
        {
            await WindowManagerService.Current.TryShowAsStandaloneAsync(pageType.Name, pageType);
        }

        public async Task OpenInDialog(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            var dialog = ShellContentDialog.Create(pageType, parameter, infoOverride);
            await dialog.ShowAsync();
        }
    }
}
