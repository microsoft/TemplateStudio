        Private Sub RefreshCommands()
            '{[{
            Dim undo = TryCast(UndoCommand, RelayCommand)
            undo?.RaiseCanExecuteChanged()
            Dim redo = TryCast(RedoCommand, RelayCommand)
            redo?.RaiseCanExecuteChanged()
            Dim saveInk = TryCast(SaveInkFileCommand, RelayCommand)
            saveInk?.RaiseCanExecuteChanged()
            Dim export = TryCast(TransformTextAndShapesCommand, RelayCommand)
            export?.RaiseCanExecuteChanged()
            Dim clearAll = TryCast(ClearAllCommand, RelayCommand)
            clearAll?.RaiseCanExecuteChanged()
            '}]}
        End Sub