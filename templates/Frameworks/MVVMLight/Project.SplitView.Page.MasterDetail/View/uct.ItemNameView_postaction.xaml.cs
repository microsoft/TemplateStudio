using Windows.UI.Xaml.Navigation;
namespace ItemNamespace.View
{
    public sealed partial class uct.ItemNameView : Page
    {
        //^^
        
        //{[{
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadDataAsync(visualStateGroup.CurrentState);
        }        
        //}]}
    }
}
