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
            NavigationService.Navigate<wts.ItemNameDetailPage>(args.ClickedItem);
        }
        //}]}
    }
}