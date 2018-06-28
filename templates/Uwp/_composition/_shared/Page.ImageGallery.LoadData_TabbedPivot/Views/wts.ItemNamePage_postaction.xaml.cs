namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
            //{[{
            Loaded += wts.ItemNamePage_Loaded;
            //}]}
        }

        //{[{
        private async void wts.ItemNamePage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAnimationAsync();
        }
        //}]}
    }
}