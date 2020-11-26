'{[{
Imports Microsoft.Toolkit.Mvvm.Input
'}]}

        Private Sub RefreshCommands()
            '{[{
            Dim saveCommand = TryCast(SaveImageCommand, RelayCommand)
            saveCommand?.NotifyCanExecuteChanged()
            Dim clearCommand = TryCast(ClearAllCommand, RelayCommand)
            clearCommand?.NotifyCanExecuteChanged()
            '}]}
        End Sub
