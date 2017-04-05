using Windows.UI.Xaml.Navigation;
namespace ItemNamespace.Views
{
    public sealed partial class uct.ItemNamePage : Page, INotifyPropertyChanged
    {
        
        public uct.ItemNamePage()
        {
        }  
        
        //{[{        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //TODO UWPTemplates: Set your map service token. If you don't have it, request at https://www.bingmapsportal.com/            
            mapControl.MapServiceToken = "";
            
            AddMapIcon(Center, "Microsoft Corporation");
        }        
        //}]}
    }
}
