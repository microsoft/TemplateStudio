//{[{
using Microsoft.Toolkit.Mvvm.Input;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ts.ItemNameViewModel : ObservableObject
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
