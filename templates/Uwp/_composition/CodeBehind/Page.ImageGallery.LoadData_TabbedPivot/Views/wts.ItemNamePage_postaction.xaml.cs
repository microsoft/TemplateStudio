namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
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
            var selectedImageId = ImagesNavigationHelper.GetImageId(wts.ItemNameSelectedIdKey);
            if (!string.IsNullOrEmpty(selectedImageId))
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(wts.ItemNameAnimationClose);
                if (animation != null)
                {
                    var item = ImagesGridView.Items.FirstOrDefault(i => ((SampleImage)i).ID == selectedImageId);
                    ImagesGridView.ScrollIntoView(item);
                    await ImagesGridView.TryStartConnectedAnimationAsync(animation, item, "galleryImage");
                }

                ImagesNavigationHelper.RemoveImageId(wts.ItemNameSelectedIdKey);
            }
        }
        //}]}
    }
}