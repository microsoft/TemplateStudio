//{[{
using Param_RootNamespace.Views;
//}]}
using System;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Observable
    {
        private void OnItemSelected(ItemClickEventArgs args)
        {
//^^
//{[{
            NavigationService.Navigate<wts.ItemNameDetailPage>(selected.ID);
//}]}
        }
    }
}