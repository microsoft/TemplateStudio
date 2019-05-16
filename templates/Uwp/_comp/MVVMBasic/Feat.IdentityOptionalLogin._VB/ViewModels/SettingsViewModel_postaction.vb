Namespace ViewModels
    Public Class SettingsViewModel
        Inherits Observable

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [Set](_isBusy, value)
'^^
'{[{
                LogInCommand.OnCanExecuteChanged()
                LogOutCommand.OnCanExecuteChanged()
'}]}
            End Set
        End Property
    End Class
End Namespace
