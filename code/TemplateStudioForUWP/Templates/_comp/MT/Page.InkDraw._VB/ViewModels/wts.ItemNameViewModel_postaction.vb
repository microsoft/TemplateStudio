'{[{
Imports Microsoft.Toolkit.Mvvm.Input
'}]}

        Private Sub RefreshCommands()
            '{[{
            Dim copy = TryCast(CopyCommand, RelayCommand)
            copy?.NotifyCanExecuteChanged()
            Dim cut = TryCast(CutCommand, RelayCommand)
            cut?.NotifyCanExecuteChanged()
            Dim paste = TryCast(PasteCommand, RelayCommand)
            paste?.NotifyCanExecuteChanged()
            Dim undo = TryCast(UndoCommand, RelayCommand)
            undo?.NotifyCanExecuteChanged()
            Dim redo = TryCast(RedoCommand, RelayCommand)
            redo?.NotifyCanExecuteChanged()
            Dim saveInk = TryCast(SaveInkFileCommand, RelayCommand)
            saveInk?.NotifyCanExecuteChanged()
            Dim export = TryCast(ExportAsImageCommand, RelayCommand)
            export?.NotifyCanExecuteChanged()
            Dim clearAll = TryCast(ClearAllCommand, RelayCommand)
            clearAll?.NotifyCanExecuteChanged()
            '}]}
        End Sub
