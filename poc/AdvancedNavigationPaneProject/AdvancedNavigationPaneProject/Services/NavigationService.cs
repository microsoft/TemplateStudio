using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace AdvancedNavigationPaneProject.Services
{
    public enum NavigationFrame
    {
        Main,
        Secondary
    }

    public static class NavigationService
    {
        public static event EventHandler<NavigationEventArgsEx> Navigated;

        public static event NavigationFailedEventHandler NavigationFailed;

        private static bool _isFirstNewNavigation;
        private static Frame _mainFrame;
        private static Frame _secondaryFrame;
        private static List<(NavigationFrame NavigationFrame, PageStackEntry PageStackEntry)> _backStack = new List<(NavigationFrame, PageStackEntry)>();

        public static void InitializeMainFrame(Frame mainFrame)
        {
            _mainFrame = mainFrame;
            _mainFrame.Navigated += OnMainFrameNavigated;
            _mainFrame.NavigationFailed += OnMainFrameNavigationFailed;
            Window.Current.Content = _mainFrame;
        }

        public static void InitializeSecondaryFrame(Frame secondaryFrame)
        {
            _secondaryFrame = secondaryFrame;
            _secondaryFrame.Navigated += OnSecondaryFrameNavigated;
            _secondaryFrame.NavigationFailed += OnSecondaryFrameNavigationFailed;
        }

        public static bool IsInitialized(NavigationFrame navigationFrame)
        {
            var frame = navigationFrame == NavigationFrame.Main ? _mainFrame : _secondaryFrame;
            return frame?.Content != null;
        }

        public static bool CanGoBack => _backStack.Any();

        public static void GoBack()
        {
            if (CanGoBack)
            {
                var pageStackEntry = _backStack.First();
                var frame = GetActiveFrame(pageStackEntry.NavigationFrame);
                frame.GoBack();
            }
        }

        public static bool Navigate<T>()
            where T : Page
            => Navigate<T>(null);

        public static bool Navigate<T>(NavigateConfig config)
            where T : Page
            => Navigate(typeof(T), config);

        public static bool Navigate(Type pageType)
            => Navigate(pageType, null);

        public static bool Navigate(Type pageType, NavigateConfig config = null)
        {
            config = config ?? NavigateConfig.Defaul;
            var frame = GetActiveFrame(config.NavigationFrame);
            _isFirstNewNavigation = frame.Content == null || frame.Content.GetType() == pageType;
            return frame.Navigate(pageType, config.Parameter, config.InfoOverride);
        }

        private static Frame GetActiveFrame(NavigationFrame navigationFrame)
        {
            var frame = navigationFrame == NavigationFrame.Main ? _mainFrame : _secondaryFrame;
            if (frame == null)
            {
                var methodName = navigationFrame == NavigationFrame.Main ? nameof(InitializeMainFrame) : nameof(InitializeSecondaryFrame);
                throw new Exception($"Frame is not initialized, please call {methodName} before navigate.");
            }
            return frame;
        }

        private static void OnMainFrameNavigated(object sender, NavigationEventArgs e)
        {
            SetupNavigated(NavigationFrame.Main, _mainFrame, e);
        }

        private static void OnMainFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private static void OnSecondaryFrameNavigated(object sender, NavigationEventArgs e)
        {
            SetupNavigated(NavigationFrame.Secondary, _secondaryFrame, e);
        }

        private static void OnSecondaryFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private static void SetupNavigated(NavigationFrame navigationFrame, Frame frame, NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New && !_isFirstNewNavigation)
            {
                _backStack.Insert(0, (navigationFrame, new PageStackEntry(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo)));
                _isFirstNewNavigation = frame.Content == null;
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                _backStack.RemoveAt(0);
            }
            Navigated?.Invoke(frame, new NavigationEventArgsEx(navigationFrame, e));
        }
    }
}
