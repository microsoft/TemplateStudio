using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AdvancedNavigationPaneProject.Services
{
    public static class NavigationService
    {
        public const string FrameKeyMain = "Main";
        public const string FrameKeySecondary = "Secondary";
        public const string FrameKeyThird = "Third";

        public static event EventHandler<NavigationEventArgsEx> Navigated;

        public static event NavigationFailedEventHandler NavigationFailed;

        private static Dictionary<string, Frame> _frames = new Dictionary<string, Frame>();
        private static List<PageStackEntryEx> _backStack = new List<PageStackEntryEx>();

        public static bool InitializeMainFrame(Frame mainFrame)
        {
            if(InitializeFrame(FrameKeyMain, mainFrame))
            {
                Window.Current.Content = mainFrame;
                return true;
            }
            return false;
        }

        public static bool InitializeFrame(string frameKey, Frame frame)
        {
            if (!_frames.ContainsKey(frameKey))
            {
                frame.Tag = frameKey;
                frame.Navigated += OnFrameNavigated;
                frame.NavigationFailed += OnFrameNavigationFailed;
                _frames.Add(frameKey, frame);
                return true;
            }
            return false;
        }

        public static bool IsInitialized(string frameKey)
        {
            var frame = _frames.GetValueOrDefault(frameKey);
            return frame?.Content != null;
        }

        public static bool CanGoBack => _backStack.Any();

        public static void GoBack()
        {
            if (CanGoBack)
            {
                var stackEntry = _backStack.First();
                var frame = GetFrame(stackEntry.FrameKey);
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
            config = config ?? NavigateConfig.Default;
            var frame = GetFrame(config.FrameKey);
            if (frame.Content == null || frame.Content.GetType() != pageType)
            {
                return frame.Navigate(pageType, config.Parameter, config.InfoOverride);
            }
            return false;
        }

        public static void RestartNavigation()
        {
            foreach (var frame in _frames.Values)
            {
                frame.Navigated -= OnFrameNavigated;
                frame.NavigationFailed -= OnFrameNavigationFailed;
            }
            _frames.Clear();
            _backStack.Clear();
            InitializeMainFrame(new Frame());
        }

        public static Frame GetFrame(string frameKey)
        {
            var frame = _frames.GetValueOrDefault(frameKey);
            if (frame == null)
            {
                var methodName = frameKey == FrameKeyMain ? nameof(InitializeMainFrame) : nameof(InitializeFrame);
                throw new Exception($"Frame is not initialized, please call {methodName} before navigate.");
            }
            return frame;
        }

        private static void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                var frameKey = frame.Tag as string;
                if (e.NavigationMode == NavigationMode.New && frame.CanGoBack)
                {
                    _backStack.Insert(0, new PageStackEntryEx(frameKey, e));
                }
                else if (e.NavigationMode == NavigationMode.Back)
                {
                    _backStack.RemoveAt(0);
                }
                Navigated?.Invoke(frame, new NavigationEventArgsEx(frameKey, e));
            }
        }

        private static void OnFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }
    }
}
