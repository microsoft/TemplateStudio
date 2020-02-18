namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : Observable
    {
//^^
//{[{

        private void OnNavigated(object sender, string viewModelName)
            => GoBackCommand.OnCanExecuteChanged();
//}]}
    }
}
