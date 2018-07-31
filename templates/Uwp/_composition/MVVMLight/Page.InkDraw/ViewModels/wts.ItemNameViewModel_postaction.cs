        private void RefreshCommands()
        {
            //{[{
            (CopyCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (CutCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (PasteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (SaveInkFileCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ExportAsImageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.RaiseCanExecuteChanged();
            //}]}
        }