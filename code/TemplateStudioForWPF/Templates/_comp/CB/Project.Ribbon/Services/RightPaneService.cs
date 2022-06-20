using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;

namespace Param_RootNamespace.Services;

public class RightPaneService : IRightPaneService
{
    private readonly IServiceProvider _serviceProvider;
    private Frame _frame;
    private object _lastParameterUsed;
    private SplitView _splitView;

    public event EventHandler PaneOpened;

    public event EventHandler PaneClosed;

    public RightPaneService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Initialize(Frame rightPaneFrame, SplitView splitView)
    {
        _frame = rightPaneFrame;
        _splitView = splitView;
        _frame.Navigated += OnNavigated;
        _splitView.PaneClosed += OnPaneClosed;
    }

    public void CleanUp()
    {
        _frame.Navigated -= OnNavigated;
        _splitView.PaneClosed -= OnPaneClosed;
    }

    public void OpenInRightPane(Type pageType, object parameter = null)
    {
        if (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed)))
        {
            var page = _serviceProvider.GetService(pageType) as Page;
            var navigated = _frame.Navigate(page, parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
                if (_frame.Content is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }
        }

        _splitView.IsPaneOpen = true;
        PaneOpened?.Invoke(_splitView, EventArgs.Empty);
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            frame.CleanNavigation();
            if (_frame.Content is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.ExtraData);
            }
        }
    }

    private void OnPaneClosed(object sender, EventArgs e)
        => PaneClosed?.Invoke(sender, e);
}
