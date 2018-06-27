Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
            '{[{
            AddHandler Loaded, AddressOf wts.ItemNamePage_Loaded
            '}]}
        End Sub

        '{[{
        Private Async Sub wts.ItemNamePage_Loaded(sender As Object, e As RoutedEventArgs)
            Await ViewModel.LoadAnimationAsync()
        End Sub
        '}]}
    End Class
End Namespace
