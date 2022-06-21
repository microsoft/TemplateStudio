using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;

namespace Param_RootNamespace.Services;

public class WindowManagerService : IWindowManagerService
{
    private readonly IServiceProvider _serviceProvider;

    public Window MainWindow
        => Application.Current.MainWindow;

    public WindowManagerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void OpenInNewWindow(Type pageType, object parameter = null)
    {
        var window = GetWindow(pageType);
        if (window != null)
        {
            window.Activate();
        }
        else
        {
            window = new MetroWindow()
            {
                Title = "Param_ProjectName",
                Style = Application.Current.FindResource("CustomMetroWindow") as Style
            };
            var frame = new Frame()
            {
                Focusable = false,
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };

            window.Content = frame;
            window.Closed += OnWindowClosed;
            window.Show();
            frame.Navigated += OnNavigated;
            var page = _serviceProvider.GetService(pageType);
            var navigated = frame.Navigate(page, parameter);
        }
    }

    public bool? OpenInDialog(Type pageType, object parameter = null)
    {
        var shellWindow = _serviceProvider.GetService(typeof(IShellDialogWindow)) as Window;
        var frame = ((IShellDialogWindow)shellWindow).GetDialogFrame();
        frame.Navigated += OnNavigated;
        shellWindow.Closed += OnWindowClosed;
        var page = _serviceProvider.GetService(pageType);
        var navigated = frame.Navigate(page, parameter);
        return shellWindow.ShowDialog();
    }

    public Window GetWindow(Type pageType)
    {
        foreach (Window window in Application.Current.Windows)
        {
            if (window.Content is Frame frame)
            {
                if (frame.Content.GetType() == pageType)
                {
                    return window;
                }
            }
        }

        return null;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var page = frame.Content;
            if (page is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.ExtraData);
            }
        }
    }

    private void OnWindowClosed(object sender, EventArgs e)
    {
        if (sender is Window window)
        {
            if (window.Content is Frame frame)
            {
                frame.Navigated -= OnNavigated;
            }

            window.Closed -= OnWindowClosed;
        }
    }
}
