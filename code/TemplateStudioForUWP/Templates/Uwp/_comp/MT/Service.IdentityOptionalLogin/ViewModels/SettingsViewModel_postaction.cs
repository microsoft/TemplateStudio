//{[{
using Microsoft.Toolkit.Mvvm.Input;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
//^^
//{[{
                LogInCommand.NotifyCanExecuteChanged();
                LogOutCommand.NotifyCanExecuteChanged();
//}]}
            }
        }
    }
}
