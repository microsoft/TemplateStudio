using Microsoft.Practices.ServiceLocation;
namespace ItemNamespace.ItemName
{
    public sealed partial class ItemNamePage : Page
    {
        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //^^
            var ViewModel = ServiceLocator.Current.GetInstance<ItemNameViewModel>();
            ViewModel.SetMap(map);
        }
    }
}
