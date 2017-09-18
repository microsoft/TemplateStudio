Imports Windows.UI.Xaml.Controls
Imports System.Collections.ObjectModel
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services
Imports System.ComponentModel

Namespace Views
    Partial Public NotInheritable Class GridViewPage
        Inherits Page
        Implements INotifyPropertyChanged

        ' TODO WTS: Change the grid as appropriate to your app.
        ' For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        ' You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted

        Public ReadOnly Property Source As ObservableCollection(Of SampleOrder)
            Get
                ' TODO WTS: Replace this with your actual data
                Return SampleDataService.GetGridSampleData()
            End Get
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
