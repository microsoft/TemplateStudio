using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.Services;

// For more information on navigation between pages see
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/navigation.md
public class MockNavigationService : INavigationService
{
    private Frame _frame;

    public event NavigatedEventHandler Navigated;

    public Frame Frame
    {
        get => _frame;
        set
        {
        }
    }
    public bool CanGoBack => Frame.CanGoBack;

    public MockNavigationService(IPageService pageService)
    {
    }

    public bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false)
    {
        return true;
    }

    public bool GoBack()
    {
        return true;
    }

    public void SetListDataItemForNextConnectedAnimation(object item)
    {
    }


}
