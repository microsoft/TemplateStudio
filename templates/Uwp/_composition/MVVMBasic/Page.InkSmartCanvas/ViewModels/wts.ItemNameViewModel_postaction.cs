        private void RefreshCommands()
        {
            //{[{
            (UndoCommand as RelayCommand)?.OnCanExecuteChanged();
            (RedoCommand as RelayCommand)?.OnCanExecuteChanged();
            (SaveInkFileCommand as RelayCommand)?.OnCanExecuteChanged();
            (TransformTextAndShapesCommand as RelayCommand)?.OnCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.OnCanExecuteChanged();
            //}]}
        }