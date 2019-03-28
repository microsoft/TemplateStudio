        private void RefreshCommands()
        {
            //{[{
            (SaveImageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ClearAllCommand as RelayCommand)?.RaiseCanExecuteChanged();
            //}]}
        }