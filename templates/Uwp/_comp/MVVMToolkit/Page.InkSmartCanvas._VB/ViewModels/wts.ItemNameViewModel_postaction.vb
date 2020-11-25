'{[{
Imports Microsoft.Toolkit.Mvvm.Input
'}]}

        Private Sub RefreshCommands()
            '{[{
            Dim undo = TryCast(UndoCommand, RelayCommand)
            undo?.NotifyCanExecuteChanged()
            Dim redo = TryCast(RedoCommand, RelayCommand)
            redo?.NotifyCanExecuteChanged()
            Dim saveInk = TryCast(SaveInkFileCommand, RelayCommand)
            saveInk?.NotifyCanExecuteChanged()
            Dim export = TryCast(TransformTextAndShapesCommand, RelayCommand)
            export?.NotifyCanExecuteChanged()
            Dim clearAll = TryCast(ClearAllCommand, RelayCommand)
            clearAll?.NotifyCanExecuteChanged()
            '}]}
        End Sub
