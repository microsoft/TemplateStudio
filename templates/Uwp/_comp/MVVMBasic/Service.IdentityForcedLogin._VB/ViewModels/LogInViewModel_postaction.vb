Namespace ViewModels
    Public Class LogInViewModel

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [Set](_isBusy, value)
'{[{
                LoginCommand.OnCanExecuteChanged()
'}]}
            End Set
        End Property
    End Class
End Namespace
