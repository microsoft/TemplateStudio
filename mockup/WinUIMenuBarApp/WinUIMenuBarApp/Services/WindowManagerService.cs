using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIMenuBarApp.Contracts.Services;
using WinUIMenuBarApp.Contracts.ViewModels;
using WinUIMenuBarApp.Contracts.Views;
using WinUIMenuBarApp.Helpers;

namespace WinUIMenuBarApp.Services
{
    public class WindowManagerService : IWindowManagerService
    {
        private readonly IPageService _pageService;

        public WindowManagerService(IPageService pageService)
        {
            _pageService = pageService;
        }
        public async Task OpenInDialogAsync(string pageKey, object parameter = null)
        {
            var dialog = Ioc.Default.GetService<IShellContentDialog>();
            var contentDialog = dialog as ContentDialog;
            var frame = dialog.GetDialogFrame();
            frame.Navigated += OnNavigated;
            contentDialog.Closed += OnDialogClosed;
            var pageType = _pageService.GetPageType(pageKey);
            var vmBeforeNavigation = frame.GetPageViewModel();
            var navigated = frame.Navigate(pageType, parameter);
            if (navigated)
            {
                if (vmBeforeNavigation is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

            await contentDialog.ShowAsync();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                if (frame.GetPageViewModel() is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(e.Parameter);
                }
            }
        }

        private void OnDialogClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (sender is IShellContentDialog dialog)
            {
                dialog.GetDialogFrame().Navigated -= OnNavigated;
            }

            sender.Closed -= OnDialogClosed;
        }
    }
}
