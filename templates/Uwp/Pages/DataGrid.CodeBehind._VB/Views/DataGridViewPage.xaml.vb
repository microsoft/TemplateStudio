Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class DataGridViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _source As ObservableCollection(Of SampleOrder)

        ' TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on DataGridViewPage.xaml.
        ' For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        Public Sub New()
            InitializeComponent()
        End Sub

        Public Property Source As ObservableCollection(Of SampleOrder)
            Get
                Return _source
            End Get
            Set(value As ObservableCollection(Of SampleOrder))
                [Set](_source, value)
            End Set
        End Property

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)

            ' TODO WTS: Replace this with your actual data
            Source = Await SampleDataService.GetGridSampleDataAsync()
        End Sub
    End Class
End Namespace
