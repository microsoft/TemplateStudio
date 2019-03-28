namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : Screen
    {

        public wts.ItemNameDetailViewModel(INavigationService navigationService, IConnectedAnimationService connectedAnimationService)
        {
        }
//{[{

        public void GoBack()
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
            }
        }
//}]}
    }
}
