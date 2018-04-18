Namespace Views
    Public NotInheritable Partial Class MasterDetailViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, AddressOf MasterDetailViewPage_Loaded
        End Sub

        Private Async Sub MasterDetailViewPage_Loaded(sender As Object, e As RoutedEventArgs)
            Await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState)
        End Sub
    End Class
End Namespace
