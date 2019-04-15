'{[{
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
'}]}
Namespace ViewModels
    Public Class LogInViewModel
        Inherits ViewModelBase

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [Set](_isBusy, value)
'{[{
                LoginCommand.RaiseCanExecuteChanged()
'}]}
            End Set
        End Property
    End Class
End Namespace
