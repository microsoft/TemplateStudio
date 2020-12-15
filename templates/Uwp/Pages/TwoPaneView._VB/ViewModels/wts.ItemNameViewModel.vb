Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports WinUI = Microsoft.UI.Xaml.Controls

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _twoPaneView As WinUI.TwoPaneView
        Private _selected As SampleOrder
        Private _itemClickCommand As ICommand
        Private _modeChangedCommand As ICommand
        Private _twoPanePriority As WinUI.TwoPaneViewPriority

        Public Property Selected As SampleOrder
            Get
                Return _selected
            End Get
            Set(value As SampleOrder)
                [Param_Setter](_selected, value)
            End Set
        End Property

        Public Property TwoPanePriority As WinUI.TwoPaneViewPriority
            Get
                Return _twoPanePriority
            End Get
            Set(value As WinUI.TwoPaneViewPriority)
                [Param_Setter](_twoPanePriority, value)
            End Set
        End Property

        Public Property SampleItems As ObservableCollection(Of SampleOrder) = New ObservableCollection(Of SampleOrder)()

        Public ReadOnly Property ItemClickCommand As ICommand
            Get
                If _itemClickCommand Is Nothing Then
                    _itemClickCommand = New RelayCommand(AddressOf OnItemClick)
                End If

                Return _itemClickCommand
            End Get
        End Property

        Public ReadOnly Property ModeChangedCommand As ICommand
            Get
                If _modeChangedCommand Is Nothing Then
                    _modeChangedCommand = New RelayCommand(Of WinUI.TwoPaneView)(AddressOf OnModeChanged)
                End If

                Return _modeChangedCommand
            End Get
        End Property

        Public Sub Initialize(twoPaneView As WinUI.TwoPaneView)
            _twoPaneView = twoPaneView
        End Sub

        Public Async Function LoadDataAsync() As Task
            SampleItems.Clear()
            Dim data = Await SampleDataService.GetTwoPaneViewDataAsync()

            For Each item In data
                SampleItems.Add(item)
            Next

            Selected = SampleItems.First()
        End Function

        Public Function TryCloseDetail() As Boolean
            If TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2 Then
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1
                Return True
            End If

            Return False
        End Function

        Private Sub OnItemClick()
            If _twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
            End If
        End Sub

        Private Sub OnModeChanged(twoPaneView As WinUI.TwoPaneView)
            If twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
            Else
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1
            End If
        End Sub
    End Class
End Namespace