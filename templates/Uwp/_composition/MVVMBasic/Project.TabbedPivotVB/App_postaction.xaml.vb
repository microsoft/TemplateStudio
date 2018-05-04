NotInheritable Partial Class App
    Inherits Application
'^^
'{[{

    Private Function CreateActivationService() As ActivationService
        Return New ActivationService(Me, GetType(Views.PivotPage))
    End Function
'}]}
End Class
