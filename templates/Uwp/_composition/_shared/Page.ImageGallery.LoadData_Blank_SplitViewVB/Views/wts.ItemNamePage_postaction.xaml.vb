Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
        End Sub

        '{[{
        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            If e.NavigationMode = NavigationMode.Back Then
                Await ViewModel.LoadAnimationAsync()
            End If
        End Sub
        '}]}
    End Class
End Namespace
