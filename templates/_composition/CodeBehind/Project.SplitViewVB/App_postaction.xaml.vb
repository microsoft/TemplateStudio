NotInheritable Partial Class App
    Inherits Application
'^^
'{[{

    Private Function CreateActivationService() As ActivationService
        Return New ActivationService(Me, GetType(Views.Param_HomeNamePage), New Lazy(Of UIElement)(AddressOf CreateShell))
    End Function

    Private Function CreateShell() As UIElement
        Return New Views.ShellPage()
    End Function
'}]}
End Class
