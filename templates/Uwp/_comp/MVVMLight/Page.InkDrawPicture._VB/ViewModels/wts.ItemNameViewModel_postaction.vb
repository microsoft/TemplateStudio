        Private Sub RefreshCommands()
            '{[{
            Dim saveCommand = TryCast(SaveImageCommand, RelayCommand)
            saveCommand?.RaiseCanExecuteChanged()
            Dim clearCommand = TryCast(ClearAllCommand, RelayCommand)
            clearCommand?.RaiseCanExecuteChanged()
            '}]}
        End Sub