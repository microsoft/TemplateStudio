//{[{
using GalaSoft.MvvmLight.Command;
//}]}

namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
//{[{
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

//}]}

        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
//{[{
                NavigationService.Navigate(typeof(ContentGridDetailViewModel).FullName, clickedItem);
//}]}
            }
        }
    }
}