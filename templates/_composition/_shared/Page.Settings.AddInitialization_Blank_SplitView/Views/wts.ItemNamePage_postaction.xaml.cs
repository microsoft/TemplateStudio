using Windows.UI.Xaml.Navigation;
namespace ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        //^^

        //{[{
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Initialize();
        }
        //}]}
    }
}
