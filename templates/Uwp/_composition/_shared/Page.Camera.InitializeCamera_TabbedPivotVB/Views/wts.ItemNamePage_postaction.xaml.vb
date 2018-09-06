'{[{
Imports Param_ItemNamespace.Helpers
'}]}

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
        End Sub

        '{[{
        Public Async Function OnPivotSelectedAsync() As Task Implements IPivotPage.OnPivotSelectedAsync
            Await cameraControl.InitializeCameraAsync()
        End Function

        Public Async Function OnPivotUnselectedAsync() As Task Implements IPivotPage.OnPivotUnselectedAsync
            Await cameraControl.CleanupCameraAsync()
        End Function
        '}]}
    End Class
End Namespace
