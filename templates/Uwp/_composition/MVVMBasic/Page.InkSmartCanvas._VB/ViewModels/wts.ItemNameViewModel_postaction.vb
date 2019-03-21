        Private Sub RefreshCommands()
            '{[{
            Dim undo = TryCast(UndoCommand, RelayCommand)
            undo?.OnCanExecuteChanged()
            Dim redo = TryCast(RedoCommand, RelayCommand)
            redo?.OnCanExecuteChanged()
            Dim saveInk = TryCast(SaveInkFileCommand, RelayCommand)
            saveInk?.OnCanExecuteChanged()
            Dim export = TryCast(TransformTextAndShapesCommand, RelayCommand)
            export?.OnCanExecuteChanged()
            Dim clearAll = TryCast(ClearAllCommand, RelayCommand)
            clearAll?.OnCanExecuteChanged()
            '}]}
        End Sub
