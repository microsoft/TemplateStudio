'{[{
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
'}]}
Namespace ViewModels
    Public Class SettingsViewModel
        Inherits ViewModelBase

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [Set](_isBusy, value)
'^^
'{[{
                LogInCommand.RaiseCanExecuteChanged()
                LogOutCommand.RaiseCanExecuteChanged()
'}]}
            End Set
        End Property
    End Class
End Namespace
