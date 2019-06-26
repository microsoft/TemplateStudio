Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class GridViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _source As ObservableCollection(Of SampleOrder)

        ' TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on GridViewPage.xaml.
        ' For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        ' You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
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
            Source = Await SampleDataService.GetGridDataAsync()
        End Sub
    End Class
End Namespace
