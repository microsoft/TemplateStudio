//{[{
using Param_RootNamespace.Views;
using Microsoft.Toolkit.Mvvm.Input;
//}]}
using System;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ObservableObject
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