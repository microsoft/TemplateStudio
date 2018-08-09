        Private Sub RefreshCommands()
            '{[{
            Dim saveCommand = TryCast(SaveImageCommand, RelayCommand)
            saveCommand?.OnCanExecuteChanged()
            Dim clearCommand = TryCast(ClearAllCommand, RelayCommand)
            clearCommand?.OnCanExecuteChanged()
            '}]}
        End Sub