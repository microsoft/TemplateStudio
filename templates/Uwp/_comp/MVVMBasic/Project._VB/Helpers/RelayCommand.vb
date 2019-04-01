Namespace Helpers
    Public Class RelayCommand
        Implements ICommand

        Private ReadOnly _execute As Action

        Private ReadOnly _canExecute As Func(Of Boolean)

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Sub New(execute As Action)
            Me.New(execute, Nothing)
        End Sub

        Public Sub New(execute As Action, canExecute As Func(Of Boolean))
            If execute Is Nothing Then
                Throw New ArgumentNullException(NameOf(execute))
            End If

            Me._execute = execute
            Me._canExecute = canExecute
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            If _canExecute Is Nothing Then
                Return True
            Else
                Return _canExecute()
            End If
        End Function

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            _execute()
        End Sub

        Public Sub OnCanExecuteChanged()
            RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
        End Sub
    End Class

    Public Class RelayCommand(Of T)
        Implements ICommand

        Private ReadOnly _execute As Action(Of T)

        Private ReadOnly _canExecute As Func(Of T, Boolean)

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Sub New(execute As Action(Of T))
            Me.New(execute, Nothing)
        End Sub

        Public Sub New(execute As Action(Of T), canExecute As Func(Of T, Boolean))
            If execute Is Nothing Then
                Throw New ArgumentNullException(NameOf(execute))
            End If

            Me._execute = execute
            Me._canExecute = canExecute
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            If _canExecute Is Nothing Then
                Return True
            Else
                Return _canExecute(parameter)
            End If
        End Function

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            _execute(parameter)
        End Sub

        Public Sub OnCanExecuteChanged()
            RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
        End Sub
    End Class
End Namespace
