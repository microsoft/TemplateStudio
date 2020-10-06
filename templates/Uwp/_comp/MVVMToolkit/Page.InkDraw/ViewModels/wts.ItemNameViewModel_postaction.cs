//{[{
using Microsoft.Toolkit.Mvvm.Input;
//}]}

        private void RefreshCommands()
        {
            //{[{
            (CopyCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (CutCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (PasteCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (UndoCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (RedoCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (SaveInkFileCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (ExportAsImageCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.NotifyCanExecuteChanged();
            //}]}
        }