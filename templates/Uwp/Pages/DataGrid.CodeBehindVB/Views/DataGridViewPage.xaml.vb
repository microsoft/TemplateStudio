Imports Param_ItemNamespace.Core.Models
Imports Param_ItemNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class DataGridViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        ' TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on DataGridViewPage.xaml.
        ' For more details see the documentation at https://github.com/Microsoft/WindowsCommunityToolkit/blob/harinikmsft/datagrid/docs/controls/DataGrid.md
        Public Sub New()
            InitializeComponent()
        End Sub

        Public ReadOnly Property Source As ObservableCollection(Of SampleOrder)
            Get
                ' TODO WTS: Replace this with your actual data
                Return SampleDataService.GetGridSampleData()
            End Get
        End Property
    End Class
End Namespace
