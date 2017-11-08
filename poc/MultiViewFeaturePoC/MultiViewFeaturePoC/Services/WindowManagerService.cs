using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiViewFeaturePoC.Services
{
    public delegate void ViewClosedHandler(ViewLifetimeControl viewControl, EventArgs e);

    public static class WindowManagerService
    {
        public static event ViewClosedHandler OnWindowClose;
        public static readonly Window MainWindow;

        public static readonly ApplicationView MainView;

        public static readonly ObservableCollection<ViewLifetimeControl> SecondaryViews;

        public static CoreWindow CurrentWindow => CoreWindow.GetForCurrentThread();

        public static CoreApplicationView CurrentView => CoreApplication.GetCurrentView();

        static WindowManagerService()
        {
            MainView = ApplicationView.GetForCurrentView();
            MainWindow = Window.Current;
            SecondaryViews = new ObservableCollection<ViewLifetimeControl>();
        }

        public static async Task<ViewLifetimeControl> TryShowAsStandaloneAsync(string windowTitle, Type pageType, object parameter = null, Action onCloseAction = null)
        {
            ViewLifetimeControl viewControl = SecondaryViews.FirstOrDefault(x => x.Title == windowTitle);
            if (viewControl == null)
            {
                viewControl = await CreateNewView(windowTitle, pageType, parameter, onCloseAction);
                viewControl.StartViewInUse();
                var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewControl.ViewId);
                viewControl.StopViewInUse();
            }
            else
            {
                await SwitchAsync(viewControl);
            }
            return viewControl;
        }

        public static async Task<ViewLifetimeControl> TryShowAsViewModeAsync(string windowTitle, Type pageType, object parameter = null, ApplicationViewMode applicationViewMode = ApplicationViewMode.CompactOverlay, Action onCloseAction = null)
        {
            ViewLifetimeControl viewControl = SecondaryViews.FirstOrDefault(x => x.Title == windowTitle);
            if (viewControl == null)
            {
                viewControl = await CreateNewView(windowTitle, pageType, parameter, onCloseAction);
                viewControl.StartViewInUse();
                var viewShown = await ApplicationViewSwitcher.TryShowAsViewModeAsync(viewControl.ViewId, applicationViewMode);
                viewControl.StopViewInUse();
            }
            else
            {
                await SwitchAsync(viewControl);
            }
            return viewControl;
        }

        public static async Task RunOnMainThreadAsync(CoreDispatcherPriority priority, DispatchedHandler agileCallback) => await MainWindow.Dispatcher.RunAsync(priority, agileCallback);

        public static async Task RunOnCurrentThreadAsync(CoreDispatcherPriority priority, DispatchedHandler agileCallback) => await CurrentWindow.Dispatcher.RunAsync(priority, agileCallback);

        public static bool IsWindowOpen(string windowTitle) => SecondaryViews.Any(v => v.Title == windowTitle);

        private static async Task<ViewLifetimeControl> CreateNewView(string windowTitle, Type pageType, object parameter = null, Action onCloseAction = null)
        {
            ViewLifetimeControl viewControl = null;
            var newView = CoreApplication.CreateNewView();
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewControl = ViewLifetimeControl.CreateForCurrentView();
                viewControl.Title = windowTitle;
                viewControl.OnCloseAction = onCloseAction;
                viewControl.Released += ViewControl_Released;
                viewControl.StartViewInUse();
                var frame = new Frame();
                frame.Navigate(pageType, viewControl);
                Window.Current.Content = frame;
                Window.Current.Activate();
                var applicationview = ApplicationView.GetForCurrentView();
                applicationview.Title = viewControl.Title;
            });
            SecondaryViews.Add(viewControl);
            return viewControl;
        }

        private static async Task SwitchAsync(ViewLifetimeControl viewControl)
        {
            await viewControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await ApplicationViewSwitcher.SwitchAsync(viewControl.ViewId, MainView.Id, ApplicationViewSwitchingOptions.Default);
            });
        }

        private static void ViewControl_Released(object sender, EventArgs e)
        {
            var viewControl = sender as ViewLifetimeControl;
            viewControl.Released -= ViewControl_Released;
            SecondaryViews.Remove(viewControl);
            OnWindowClose?.Invoke(viewControl, EventArgs.Empty);
        }
    }
}
