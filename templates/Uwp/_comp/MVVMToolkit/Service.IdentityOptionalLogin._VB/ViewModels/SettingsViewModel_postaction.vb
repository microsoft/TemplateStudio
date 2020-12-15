Namespace ViewModels
    Public Class SettingsViewModel
        Inherits ObservableObject

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [SetProperty](_isBusy, value)
'^^
'{[{
                LogInCommand.NotifyCanExecuteChanged()
                LogOutCommand.NotifyCanExecuteChanged()
'}]}
            End Set
        End Property
    End Class
End Namespace
