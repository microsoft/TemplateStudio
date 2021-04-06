//{[{
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class LogInViewModel : ObservableObject
    {
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
//{[{
                LoginCommand.NotifyCanExecuteChanged();
//}]}
            }
        }
    }
}
