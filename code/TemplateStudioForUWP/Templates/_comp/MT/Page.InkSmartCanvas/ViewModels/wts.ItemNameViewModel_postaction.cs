//{[{
using Microsoft.Toolkit.Mvvm.Input;
//}]}

        private void RefreshCommands()
        {
            //{[{
            (UndoCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (RedoCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (SaveInkFileCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (TransformTextAndShapesCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.NotifyCanExecuteChanged();
            //}]}
        }