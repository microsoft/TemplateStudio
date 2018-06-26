namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, IImageGalleryPage
    {
        public wts.ItemNamePage()
        {
            //{[{
            Loaded += wts.ItemNamePage_Loaded;
            //}]}
        }

        public GridView GetGridView() => gridView;

        //{[{
        private async void wts.ItemNamePage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAnimationAsync();
        }
        //}]}
    }
}