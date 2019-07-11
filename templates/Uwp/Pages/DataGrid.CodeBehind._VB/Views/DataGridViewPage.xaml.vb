Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class DataGridViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Property Source As ObservableCollection(Of SampleOrder) = New ObservableCollection(Of SampleOrder)

        ' TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on DataGridViewPage.xaml.
        ' For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Source.Clear()

            ' TODO WTS: Replace this with your actual data
            Dim data = Await SampleDataService.GetGridDataAsync()
            For Each item In data
                Source.Add(item)
            Next
        End Sub
    End Class
End Namespace
