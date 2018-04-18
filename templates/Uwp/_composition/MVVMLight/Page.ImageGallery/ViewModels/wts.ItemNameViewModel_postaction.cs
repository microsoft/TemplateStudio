//{[{
using GalaSoft.MvvmLight.Command;
//}]}
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Services;
namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {

        //{[{
        public NavigationServiceEx NavigationService
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }
        //}]}

        //^^
        //{[{
        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            _imagesGridView.PrepareConnectedAnimation(wts.ItemNameAnimationOpen, selected, "galleryImage");
            NavigationService.Navigate(typeof(wts.ItemNameDetailViewModel).FullName, args.ClickedItem);
        }
        //}]}
    }
}
