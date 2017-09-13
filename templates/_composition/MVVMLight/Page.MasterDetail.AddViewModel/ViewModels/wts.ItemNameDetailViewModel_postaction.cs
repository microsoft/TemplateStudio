//{[{
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
//}]}
namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : ViewModelBase
    {
        //{[{
        public NavigationServiceEx NavigationService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }

        //}]}
    }
}
