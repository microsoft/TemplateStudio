Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class GridViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Property Source As ObservableCollection(Of SampleOrder) = New ObservableCollection(Of SampleOrder)

        ' TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on GridViewPage.xaml.
        ' For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        ' You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
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
