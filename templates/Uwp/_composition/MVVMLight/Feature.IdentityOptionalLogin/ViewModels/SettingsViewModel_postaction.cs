//{[{
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
//^^
//{[{
                LogInCommand.RaiseCanExecuteChanged();
                LogOutCommand.RaiseCanExecuteChanged();
//}]}
            }
        }
    }
}
