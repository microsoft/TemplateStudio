'{[{
Imports Windows.UI.Xaml
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged
        Public Sub New()
            '{[{
            AddHandler Loaded, AddressOf wts.ItemNamePage_Loaded
            '}]}
            InitializeComponent()
        End Sub

        '{[{
        Private Async Sub wts.ItemNamePage_Loaded(sender As Object, e As RoutedEventArgs)
            Await LoadDataAsync()
        End Sub
        '}]}
    End Class
End Namespace
