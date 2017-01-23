using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MVVMLightBlankProject.Services
{
    public class NavigationService
    {
        private Frame _frame;
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();

        public NavigationService()
        {
            _frame = Window.Current.Content as Frame;
        }

        public void SetNavigationFrame(Frame frame) => _frame = frame;

        public bool CanGoBack => _frame.CanGoBack;
        public bool CanGoForward => _frame.CanGoForward;

        public void GoBack() => _frame.GoBack();
        public void GoForward() => _frame.GoForward();

        public bool Navigate(string pageKey) => Navigate(pageKey, null);
        public bool Navigate(string pageKey, object parameter) => Navigate(pageKey, parameter, null);
        public bool Navigate(string pageKey, object parameter, NavigationTransitionInfo infoOverride)
        {
            lock (_pages)
            {
                if (!_pages.ContainsKey(pageKey))
                {
                    throw new ArgumentException($"Page not found: {pageKey}. Did you forget to call NavigationService.Configure?", "pageKey");
                }
                return _frame.Navigate(_pages[pageKey], parameter, infoOverride);
            }
        }

        public void Configure(string key, Type pageType)
        {
            lock (_pages)
            {
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in NavigationService");
                }

                if (_pages.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == pageType).Key}");
                }

                _pages.Add(key, pageType);
            }
        }
    }
}