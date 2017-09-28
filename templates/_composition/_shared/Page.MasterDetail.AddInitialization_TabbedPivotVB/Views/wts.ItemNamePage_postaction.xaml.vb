'{[{
Imports Windows.UI.Xaml
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Public Sub New()
            InitializeComponent()
            '{[{
            AddHandler Loaded, AddressOf wts.ItemNamePage_Loaded
            '}]}
        End Sub

        '{[{
        Private Async Sub wts.ItemNamePage_Loaded(sender As Object, e As RoutedEventArgs)
            Await ViewModel.LoadDataAsync(WindowStates.CurrentState)
        End Sub
        '}]}
    End Class
End Namespace
