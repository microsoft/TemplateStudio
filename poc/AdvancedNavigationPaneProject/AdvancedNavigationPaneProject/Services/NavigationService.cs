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

        public static event EventHandler<NavigationArgs> Navigated;
        public static event NavigationFailedEventHandler NavigationFailed;

        private static string _currentFrame;
        private static readonly Dictionary<string, Frame> _frames = new Dictionary<string, Frame>();
        private static readonly List<NavigationBackStackEntry> _backStack = new List<NavigationBackStackEntry>();

        public static bool InitializeMainFrame(Frame mainFrame)
        {
            if (InitializeFrame(FrameKeyMain, mainFrame))
            {
                Window.Current.Content = mainFrame;
                _currentFrame = FrameKeyMain;
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
                _currentFrame = frameKey;
                return true;
            }
            _currentFrame = frameKey;
            return false;
        }

        public static bool IsInitialized(string frameKey = null)
        {
            frameKey = frameKey ?? _currentFrame;
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
            => Navigate<T>(null, null);

        public static bool Navigate<T>(string frameKey)
            where T : Page
            => Navigate<T>(frameKey, null);

        public static bool Navigate<T>(NavigationConfig config)
            where T : Page
            => Navigate<T>(null, config);

        public static bool Navigate<T>(string frameKey, NavigationConfig config)
            where T : Page
            => Navigate(typeof(T), frameKey, config);

        public static bool Navigate(Type pageType, string frameKey)
            => Navigate(pageType, frameKey, null);

        public static bool Navigate(Type pageType, NavigationConfig config)
            => Navigate(pageType, null, config);

        public static bool Navigate(Type pageType, string frameKey = null, NavigationConfig config = null)
        {
            frameKey = frameKey ?? _currentFrame;
            config = config ?? NavigationConfig.Default;
            var frame = GetFrame(frameKey);
            if (frame.Content == null || frame.Content.GetType() != pageType)
            {
                var result = frame.Navigate(pageType, config.Parameter, config.InfoOverride);
                if (result)
                {
                    if (frame.CanGoBack)
                    {
                        if (config.RegisterOnBackStack)
                        {
                            _backStack.Insert(0, new NavigationBackStackEntry(frameKey, pageType, config));
                        }
                        else
                        {
                            frame.BackStack.RemoveAt(0);
                        }
                    }
                    Navigated?.Invoke(frame, new NavigationArgs(frameKey, pageType, config, frame.Content));
                }
            }
            return false;
        }

        public static void ResetNavigation()
        {
            foreach (var frame in _frames.Values)
            {
                frame.Navigated -= OnFrameNavigated;
                frame.NavigationFailed -= OnFrameNavigationFailed;
            }
            _frames.Clear();
            _backStack.Clear();
            var newFrame = new Frame();
            InitializeMainFrame(newFrame);
            ThemeSelectorService.SetRequestedTheme();
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
                if (e.NavigationMode == NavigationMode.Back)
                {
                    _backStack.RemoveAt(0);
                }
                if (e.NavigationMode != NavigationMode.New)
                {
                    Navigated?.Invoke(frame, new NavigationArgs(frameKey, e));
                }
            }
        }

        private static void OnFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }
    }
}
