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
        Private Sub wts.ItemNamePage_Loaded(sender As Object, e As RoutedEventArgs)
            Initialize()
        End Sub
        '}]}
    End Class
End Namespace
