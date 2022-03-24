namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
//^^
//{[{

        private void OnNavigated(object sender, string viewModelName)
            => GoBackCommand.NotifyCanExecuteChanged();
//}]}
    }
}
