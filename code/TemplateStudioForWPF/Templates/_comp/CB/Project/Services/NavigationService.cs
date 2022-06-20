using System;
using System.Windows.Controls;
using System.Windows.Navigation;

using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;

namespace Param_RootNamespace.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private Frame _frame;
    private object _lastParameterUsed;

    public event EventHandler<Type> Navigated;

    public bool CanGoBack => _frame.CanGoBack;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Initialize(Frame shellFrame)
    {
        if (_frame == null)
        {
            _frame = shellFrame;
            _frame.Navigated += OnNavigated;
        }
    }

    public void UnsubscribeNavigation()
    {
        _frame.Navigated -= OnNavigated;
        _frame = null;
    }

    public void GoBack()
    {
        if (_frame.CanGoBack)
        {
            var pageBeforeNavigation = _frame.Content;
            _frame.GoBack();
            if (pageBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }
        }
    }

    public bool NavigateTo(Type pageType, object parameter = null, bool clearNavigation = false)
    {
        if (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed)))
        {
            _frame.Tag = clearNavigation;
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

            return navigated;
        }

        return false;
    }

    public void CleanNavigation()
        => _frame.CleanNavigation();

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            bool clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.CleanNavigation();
            }

            if (frame.Content is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.ExtraData);
            }

            Navigated?.Invoke(sender, frame.Content.GetType());
        }
    }
}
