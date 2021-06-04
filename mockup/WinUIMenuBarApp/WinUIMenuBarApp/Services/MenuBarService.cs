using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIMenuBarApp.Contracts.Services;
using WinUIMenuBarApp.Contracts.ViewModels;
using WinUIMenuBarApp.Helpers;

namespace WinUIMenuBarApp.Services
{
    public class MenuBarService : IMenuBarService
    {
        private readonly INavigationService _navigationService;
        private readonly IPageService _pageService;
        private SplitView _splitView;
        private Frame _rightFrame;
        private object _lastParamUsed;

        public MenuBarService(IPageService pageService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _pageService = pageService;
        }

        public void Initialize(SplitView splitView, Frame rightFrame)
        {
            _splitView = splitView;
            _rightFrame = rightFrame;
            _rightFrame.Navigated += OnNavigated;
        }

        public void UpdateView(string pageKey, object parameter = null)
            => _navigationService.NavigateTo(pageKey, parameter, true);

        public void NavigateTo(string pageKey, object parameter = null)
            => _navigationService.NavigateTo(pageKey, parameter);

        public void OpenInRightPane(string pageKey, object parameter = null)
        {
            // Don't open the same page multiple times
            if (_rightFrame.GetPageViewModel() == null || _rightFrame.GetPageViewModel().GetType().FullName != pageKey || (parameter != null && !parameter.Equals(_lastParamUsed)))
            {
                var pageType = _pageService.GetPageType(pageKey);
                var vmBeforeNavigation = _rightFrame.GetPageViewModel();
                var navigationResult = _rightFrame.Navigate(pageType, parameter);
                if (navigationResult)
                {
                    _lastParamUsed = parameter;
                    if (vmBeforeNavigation is INavigationAware navigationAware)
                    {
                        navigationAware.OnNavigatedFrom();
                    }
                }
            }

            _splitView.IsPaneOpen = true;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (sender is Frame frame && frame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }
        }

        public void Exit()
        {
            Application.Current.Exit();
        }
    }
}
