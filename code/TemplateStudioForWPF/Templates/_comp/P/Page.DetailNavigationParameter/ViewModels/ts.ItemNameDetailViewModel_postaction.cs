namespace Param_RootNamespace.ViewModels
{
    public class ts.ItemNameDetailViewModel : BindableBase, INavigationAware
    {
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
//{[{
            var parameter = navigationContext.Parameters["DetailID"];
//}]}
        }
    }
}
