using System;
using System.Windows.Input;

namespace Param_RootNamespace.ViewModels
{
    public class ShellDialogViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private ICommand _closeCommand;

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new System.Windows.Input.ICommand(OnClose));

        public Action<bool?> SetResult { get; set; }

        public ShellDialogViewModel()
        {
        }

        private void OnClose()
        {
            bool result = true;
            SetResult(result);
        }
    }
}
