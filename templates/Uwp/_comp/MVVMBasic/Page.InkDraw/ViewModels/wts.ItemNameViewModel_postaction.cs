        private void RefreshCommands()
        {
            //{[{
            (CopyCommand as RelayCommand)?.OnCanExecuteChanged();
            (CutCommand as RelayCommand)?.OnCanExecuteChanged();
            (PasteCommand as RelayCommand)?.OnCanExecuteChanged();
            (UndoCommand as RelayCommand)?.OnCanExecuteChanged();
            (RedoCommand as RelayCommand)?.OnCanExecuteChanged();
            (SaveInkFileCommand as RelayCommand)?.OnCanExecuteChanged();
            (ExportAsImageCommand as RelayCommand)?.OnCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.OnCanExecuteChanged();
            //}]}
        }