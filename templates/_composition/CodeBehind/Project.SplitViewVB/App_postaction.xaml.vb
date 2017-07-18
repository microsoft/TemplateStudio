NotInheritable Partial Class App
    Inherits Application
'^^
'{[{

    Private Function CreateActivationService() As ActivationService
        Return New ActivationService(Me, GetType(Views.Param_HomeNamePage), New Views.ShellPage())
    End Function
'}]}
End Class
