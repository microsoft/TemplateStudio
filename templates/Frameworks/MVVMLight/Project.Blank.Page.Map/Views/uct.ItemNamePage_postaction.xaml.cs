using Windows.UI.Xaml.Navigation;
namespace ItemNamespace.Views
{
    public sealed partial class uct.ItemNamePage : Page
    {
        //^^
        
        //{[{        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Initialize(mapControl);
        }        
        //}]}
    }
}
