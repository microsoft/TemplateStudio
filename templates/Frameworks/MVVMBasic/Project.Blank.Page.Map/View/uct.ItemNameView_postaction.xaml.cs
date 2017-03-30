using Windows.UI.Xaml.Navigation;
namespace ItemNamespace.View
{
    public sealed partial class uct.ItemNameView : Page
    {
        //^^
        
        //{[{        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModel == null)
            {
                throw new ArgumentNullException("ViewModel");
            }
            
            ViewModel.Initialize(mapControl);
        }        
        //}]}
    }
}
