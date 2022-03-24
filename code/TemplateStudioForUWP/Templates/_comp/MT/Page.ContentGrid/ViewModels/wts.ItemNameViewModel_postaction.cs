//{[{
using Param_RootNamespace.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ObservableObject
    {
        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
//^^
//{[{
                NavigationService.Navigate<wts.ItemNameDetailPage>(clickedItem.OrderID);
//}]}
            }
        }
    }
}