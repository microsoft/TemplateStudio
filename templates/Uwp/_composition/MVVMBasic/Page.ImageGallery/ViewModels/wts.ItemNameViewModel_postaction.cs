//{[{
using Param_RootNamespace.Views;
//}]}
using System;

namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Observable
    {

        //^^
        //{[{
        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            _imagesGridView.PrepareConnectedAnimation(wts.ItemNameAnimationOpen, selected, "galleryImage");
            ImagesNavigationHelper.AddImageId(ImageGallerySelectedIdKey, selected.ID);
            NavigationService.Navigate<wts.ItemNameDetailPage>(selected.ID);
        }
        //}]}
    }
}