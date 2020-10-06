//{[{
using Microsoft.Toolkit.Mvvm.Input;
//}]}

        private void RefreshCommands()
        {
            //{[{
            (SaveImageCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.NotifyCanExecuteChanged();
            //}]}
        }