Imports Windows.UI.Xaml.Controls
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services
Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports System.Linq
Imports System.ComponentModel

Namespace Param_ItemNamespace.Views
    Partial Public NotInheritable Class MasterDetailPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _selected As SampleOrder
        Public Property Selected As SampleOrder
            Get
                Return _selected
            End Get
            Set(value As SampleOrder)
                _selected = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Selected)))
            End Set
        End Property

        Public Property SampleItems As New ObservableCollection(Of SampleOrder)

        Private Function LoadDataAsync() As Task
            SampleItems.Clear()

            Dim data = Await SampleDataService.GetSampleModelDataAsync()

			For Each item In data
                SampleItems.Add(item)
            Next

            Selected = SampleItems.First()
        End Function

        Private Sub MasterListView_ItemClick(sender As Object, e As ItemClickEventArgs)
            Dim item = TryCast(e?.ClickedItem, SampleOrder)
            If item IsNot Nothing Then
                If WindowStates.CurrentState = NarrowState Then
                    NavigationService.Navigate(Of Views.MasterDetailDetailPage)(item)
                Else
                    Selected = item
                End If
            End If
        End Sub
    End Class
End Namespace
