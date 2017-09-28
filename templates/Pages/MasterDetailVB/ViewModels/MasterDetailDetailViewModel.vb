Imports System.Windows.Input
Imports Windows.UI.Xaml
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services

Namespace ViewModels
    Public Class MasterDetailDetailViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged
        Private Const NarrowStateName As String = "NarrowState"

        Private Const WideStateName As String = "WideState"

        Private _stateChangedCommand As ICommand

        Public ReadOnly Property StateChangedCommand As ICommand
            Get
                If _stateChangedCommand Is Nothing Then
                    _stateChangedCommand = New RelayCommand(Of VisualStateChangedEventArgs)(AddressOf OnStateChanged)
                End If

                Return _stateChangedCommand
            End Get
        End Property

        Private _item As SampleOrder

        Public Property Item As SampleOrder
            Get
                Return _item
            End Get
            Set
                [Set](_item, value)
            End Set
        End Property

        Public Sub New()
        End Sub

        Private Sub OnStateChanged(args As VisualStateChangedEventArgs)
            If args.OldState.Name = NarrowStateName AndAlso args.NewState.Name = WideStateName Then
                NavigationService.GoBack()
            End If
        End Sub
    End Class
End Namespace
