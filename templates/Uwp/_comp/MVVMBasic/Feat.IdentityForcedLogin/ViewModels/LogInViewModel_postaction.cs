namespace Param_RootNamespace.ViewModels
{
    public class LogInViewModel : Observable
    {
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
//{[{
                LoginCommand.OnCanExecuteChanged();
//}]}
            }
        }
    }
}
