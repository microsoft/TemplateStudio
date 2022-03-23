Namespace Views
    Public NotInheritable Partial Class DataGridViewPage
        Inherits Page

        ' TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on DataGridViewPage.xaml.
        ' For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)

            Await ViewModel.LoadDataAsync()
        End Sub
    End Class
End Namespace
