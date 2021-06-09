Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _selected As SampleOrder

        Public Property Selected As SampleOrder
            Get
                Return _selected
            End Get
            Set
                [Param_Setter](_selected, value)
            End Set
        End Property

        Public Property SampleItems As ObservableCollection(Of SampleOrder) = new ObservableCollection(Of SampleOrder)

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, AddressOf wts.ItemNamePage_Loaded
        End Sub

        Private Async Sub wts.ItemNamePage_Loaded(sender As Object, e As RoutedEventArgs)
            SampleItems.Clear()

            Dim data = Await SampleDataService.GetListDetailsDataAsync()

            For Each item As SampleOrder In data
                SampleItems.Add(item)
            Next

            If ListDetailsViewControl.ViewState = ListDetailsViewState.Both Then
                Selected = SampleItems.FirstOrDefault()
            End If
        End Sub
    End Class
End Namespace
