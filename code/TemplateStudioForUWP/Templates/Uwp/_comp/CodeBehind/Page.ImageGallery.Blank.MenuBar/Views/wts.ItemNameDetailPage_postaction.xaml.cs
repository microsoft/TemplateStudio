//{[{
using Windows.UI.Xaml;
//}]}

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNameDetailPage : Page, INotifyPropertyChanged
    {
//^^
//{[{
        private void OnGoBack(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

//}]}
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
