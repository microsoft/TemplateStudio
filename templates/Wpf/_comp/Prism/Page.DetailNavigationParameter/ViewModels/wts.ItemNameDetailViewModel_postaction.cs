namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : BindableBase, INavigationAware
    {
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
//{[{
            var parameter = navigationContext.Parameters["DetailID"];
//}]}
        }
    }
}
