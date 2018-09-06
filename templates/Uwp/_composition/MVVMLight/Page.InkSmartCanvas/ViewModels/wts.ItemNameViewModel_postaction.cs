        private void RefreshCommands()
        {
            //{[{
            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (SaveInkFileCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (TransformTextAndShapesCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.RaiseCanExecuteChanged();
            //}]}
        }