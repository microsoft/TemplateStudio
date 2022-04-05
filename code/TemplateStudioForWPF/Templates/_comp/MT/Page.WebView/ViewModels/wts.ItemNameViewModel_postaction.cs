//{[{
using Microsoft.Toolkit.Mvvm.Input;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ObservableObject
    {
        public void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            IsLoading = false;
//^^
//{[{
            BrowserBackCommand.NotifyCanExecuteChanged();
            BrowserForwardCommand.NotifyCanExecuteChanged();
//}]}
        }
    }
}
