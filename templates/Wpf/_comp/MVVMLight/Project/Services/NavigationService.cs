using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.ViewModels;

namespace Param_RootNamespace.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IPageService _pageService;
        private object _lastParameterUsed;

        public event EventHandler<string> Navigated;

        public Frame Frame { get; private set; }

        public bool CanGoBack
            => Frame != null && Frame.CanGoBack;

        public string CurrentPageKey
        {
            get
            {
                if (Frame.Content is FrameworkElement element)
                {
                    return element.DataContext.GetType().FullName;
                }

                return string.Empty;
            }
        }

        public NavigationService(IPageService pageService)
        {
            _pageService = pageService;
        }

        public void Initialize(Frame shellFrame)
        {
            if (Frame == null)
            {
                Frame = shellFrame;
                Frame.Navigated += OnNavigated;
            }
        }

        public void UnsubscribeNavigation()
        {
            Frame.Navigated -= OnNavigated;
            Frame = null;
        }

        public void GoBack()
            => Frame.GoBack();

        public void NavigateTo(string pageKey)
            => NavigateTo(pageKey, null, false);

        public void NavigateTo(string pageKey, object parameter)
            => NavigateTo(pageKey, parameter, false);

        public void NavigateTo(string pageKey, object parameter, bool clearNavigation)
        {
            var pageType = _pageService.GetPageType(pageKey);

            if (Frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed)))
            {
                Frame.Tag = clearNavigation;
                var page = _pageService.GetPage(pageKey);
                var navigated = Frame.Navigate(page, parameter);
                if (navigated)
                {
                    _lastParameterUsed = parameter;
                    var dataContext = Frame.GetDataContext();
                    if (dataContext is INavigationAware navigationAware)
                    {
                        navigationAware.OnNavigatedFrom();
                    }
                }
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                bool clearNavigation = (bool)frame.Tag;
                if (clearNavigation)
                {
                    frame.CleanNavigation();
                }

                var dataContext = frame.GetDataContext();
                if (dataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(e.ExtraData);
                }

                Navigated?.Invoke(sender, dataContext.GetType().FullName);
            }
        }
    }
}
