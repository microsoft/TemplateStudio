'{[{
Imports Param_ItemNamespace.Helpers
'}]}

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Sub New()
        End Sub

        '{[{
        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Await cameraControl.InitializeCameraAsync()
        End Sub

        Protected Overrides Async Sub OnNavigatedFrom(e As NavigationEventArgs)
            MyBase.OnNavigatedFrom(e)
            Await cameraControl.CleanupCameraAsync()
        End Sub
        '}]}
    End Class
End Namespace

