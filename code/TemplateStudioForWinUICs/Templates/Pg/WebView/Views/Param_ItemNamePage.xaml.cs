using Microsoft.UI.Xaml.Controls;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/
public sealed partial class Param_ItemNamePage : Page
{
    public Param_ItemNameViewModel ViewModel
    {
        get;
    }

    public Param_ItemNamePage()
    {
        ViewModel = App.GetService<Param_ItemNameViewModel>()!;
        InitializeComponent();

        ViewModel.WebViewService.Initialize(WebView);
    }
}
