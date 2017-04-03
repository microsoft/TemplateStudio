using Windows.UI.Xaml.Navigation;
namespace ItemNamespace.View
{
    public sealed partial class uct.ItemNameView : Page
    {
        //^^
        
        //{[{        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModel == null)
            {
                throw new ArgumentNullException("ViewModel");
            }
            
            await ViewModel.InitializeAsync(mapControl);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (ViewModel == null)
            {
                throw new ArgumentNullException("ViewModel");
            }

            ViewModel.Cleanup();
        }
        //}]}
    }
}
