//{[{
using Param_RootNamespace.Views;
//}]}
using System;

namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {

        //^^
        //{[{
        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            _imagesGridView.PrepareConnectedAnimation(wts.ItemNameAnimationOpen, selected, "galleryImage");
            _navigationService.Navigate(PageTokens.wts.ItemNameDetailPage, args.ClickedItem);
        }
        //}]}
    }
}