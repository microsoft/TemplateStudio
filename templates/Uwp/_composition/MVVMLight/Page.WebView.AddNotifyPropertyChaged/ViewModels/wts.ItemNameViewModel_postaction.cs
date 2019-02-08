//{[{
using GalaSoft.MvvmLight.Command;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
        private void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;
            //{[{
            RaisePropertyChanged(nameof(BrowserBackCommand));
            RaisePropertyChanged(nameof(BrowserForwardCommand));
            //}]}
        }
    }
}
