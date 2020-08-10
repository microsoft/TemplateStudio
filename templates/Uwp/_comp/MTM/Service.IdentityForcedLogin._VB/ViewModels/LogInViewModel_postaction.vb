Namespace ViewModels
    Public Class LogInViewModel

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                SetProperty(_isBusy, value)
'{[{
                LoginCommand.NotifyCanExecuteChanged()
'}]}
            End Set
        End Property
    End Class
End Namespace
