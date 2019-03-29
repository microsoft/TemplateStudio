        private void RefreshCommands()
        {
            //{[{
            (SaveImageCommand as RelayCommand)?.OnCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.OnCanExecuteChanged();
            //}]}
        }