namespace Param_RootNamespace.ViewModels
{
    public class SettingsViewModel : Observable
    {
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
//^^
//{[{
                LogInCommand.OnCanExecuteChanged();
                LogOutCommand.OnCanExecuteChanged();
//}]}
            }
        }
    }
}
