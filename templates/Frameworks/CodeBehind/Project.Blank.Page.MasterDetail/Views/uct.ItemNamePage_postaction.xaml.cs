using Windows.UI.Xaml.Navigation;
namespace ItemNamespace.Views
{
    public sealed partial class uct.ItemNamePage : Page, INotifyPropertyChanged
    {
        public uct.ItemNamePage()
        {
        }
        
        //{[{
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize(AdaptiveStates.CurrentState);
            await LoadDataAsync();
        }        
        //}]}
    }
}
