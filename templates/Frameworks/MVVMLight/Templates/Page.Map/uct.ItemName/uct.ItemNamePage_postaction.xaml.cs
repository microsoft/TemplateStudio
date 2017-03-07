using Microsoft.Practices.ServiceLocation;
namespace ItemNamespace.uct.ItemName
{
    public sealed partial class uct.ItemNamePage : Page
    {
        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //^^
            var ViewModel = ServiceLocator.Current.GetInstance<uct.ItemNameViewModel>();
            ViewModel.SetMap(map);
        }
    }
}
