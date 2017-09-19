Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Input
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services

Namespace ViewModels
    Public Class MasterDetailViewModel
        Implements System.ComponentModel.INotifyPropertyChanged
        Private Const NarrowStateName As String = "NarrowState"
        Private Const WideStateName As String = "WideState"

        Private _currentState As VisualState

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

        Private _itemClickCommand As ICommand
        Public ReadOnly Property ItemClickCommand As ICommand
            Get
                If _itemClickCommand Is Nothing Then
                    _itemClickCommand = New RelayCommand(Of ItemClickEventArgs)(AddressOf OnItemClick)
                End If
                Return _itemClickCommand
            End Get
        End Property

        Private _stateChangedCommand As ICommand
        Public ReadOnly Property StateChangedCommand As ICommand
            Get
                If _stateChangedCommand Is Nothing Then
                    _stateChangedCommand = New RelayCommand(Of VisualStateChangedEventArgs)(AddressOf OnStateChanged)
                End If
                Return _stateChangedCommand
            End Get
        End Property

        Public Property SampleItems As New ObservableCollection(Of SampleOrder)

        Public Async Function LoadDataAsync(currentState As VisualState) As Task
            _currentState = currentState
            SampleItems.Clear()

            Dim data = Await SampleDataService.GetSampleModelDataAsync()

            For Each item In data
                SampleItems.Add(item)
            Next

            Selected = SampleItems.First()
        End Function

        Private Sub OnStateChanged(args As VisualStateChangedEventArgs)
            _currentState = args.NewState
        End Sub

        Private Sub OnItemClick(args As ItemClickEventArgs)
        End Sub
    End Class
End Namespace
