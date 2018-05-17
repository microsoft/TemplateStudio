using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedNavigationPaneProject.Views;
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

        private static readonly Dictionary<string, Frame> _frames = new Dictionary<string, Frame>();
        private static readonly List<NavigationBackStackEntry> _backStack = new List<NavigationBackStackEntry>();

        /// <summary>
        /// Register the main frame in the NavigationService.
        /// </summary>
        /// <param name="mainFrame">New frame to register in the NavigationService</param>
        public static bool InitializeMainFrame(Frame mainFrame)
        {
            if (InitializeFrame(FrameKeyMain, mainFrame))
            {
                Window.Current.Content = mainFrame;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Register a frame in the NavigationService using a specific frame key.
        /// </summary>
        /// <param name="frameKey">Key that will identify the frame in the NavigationService.</param>
        /// <param name="frame">New frame to register in the NavigationService.</param>
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

        /// <summary>
        /// Gets a value that indicates whether there is a frame initialized identified with a key.
        /// </summary>
        /// <param name="frameKey">Key to identify the frame in the NavigationService.</param>
        public static bool IsInitialized(string frameKey)
        {
            var frame = _frames.GetValueOrDefault(frameKey);
            return frame?.Content != null;
        }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in back navigation history.
        /// </summary>
        public static bool CanGoBack => _backStack.Any();

        /// <summary>
        /// Navigates to the most recent item in back navigation history.
        /// </summary>
        public static void GoBack()
        {
            if (CanGoBack)
            {
                var stackEntry = _backStack.First();
                var frame = GetFrame(stackEntry.FrameKey);
                frame.GoBack();
            }
        }

        /// <summary>
        /// Navigate in a specific frame using a specific NavigationConfig.
        /// </summary>
        /// <typeparam name="T">Source Page Type for Frame navigation.</typeparam>
        /// <param name="frameKey">Key that identifies the Frame to navigate.</param>
        /// <param name="config">Parameters configuration to customize the navigation.</param>
        public static bool Navigate<T>(string frameKey, NavigationConfig config = null)
            where T : Page
            => Navigate(typeof(T), frameKey, config);

        /// <summary>
        /// Navigate in a specific frame using a specific NavigationConfig.
        /// </summary>
        /// <param name="pageType">Source Page Type for Frame navigation.</param>
        /// <param name="frameKey">Key that identifies the Frame to navigate.</param>
        /// <param name="config">Parameters configuration to customize the navigation.</param>
        public static bool Navigate(Type pageType, string frameKey, NavigationConfig config = null)
        {
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

        public static bool IsPageInFrame<T>(string frameKey)
            where T : Page
        {
            var frame = GetFrame(frameKey);
            return frame.Content != null && frame.Content is T;
        }

        private static Frame GetFrame(string frameKey)
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
