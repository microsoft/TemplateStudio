using Microsoft.UI.Xaml.Controls;

using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.Services;

public class MockNavigationViewService : INavigationViewService
{

    private NavigationView _navigationView;

    public MockNavigationViewService(INavigationService navigationService, IPageService pageService)
    {
    }

    public IList<object> MenuItems => _navigationView.MenuItems;

    public object SettingsItem => _navigationView.SettingsItem;

    public NavigationViewItem GetSelectedItem(Type pageType) => GetSelectedItem(_navigationView.MenuItems, pageType);

    private NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
    {
        return null;
    }
    public void Initialize(NavigationView navigationView)
    {
    }
    public void UnregisterEvents()
    {
    }
}
