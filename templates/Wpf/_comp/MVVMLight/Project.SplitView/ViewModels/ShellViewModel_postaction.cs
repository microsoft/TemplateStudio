namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private void OnNavigated(object sender, string viewModelName)
        {
//^^
//{[{

            GoBackCommand.RaiseCanExecuteChanged();
//}]}
        }
    }
}
