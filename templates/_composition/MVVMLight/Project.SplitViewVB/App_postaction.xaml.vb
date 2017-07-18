NotInheritable Partial Class App
    Inherits Application
'^^
'{[{

    Private Function CreateActivationService() As ActivationService
        Return New ActivationService(Me, GetType(ViewModels.Param_HomeNameViewModel), New Views.ShellPage())
    End Function
'}]}
End Class
