Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services
Imports System.ComponentModel

Namespace Views
    Partial NotInheritable Class MasterDetailDetailPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _item As SampleOrder
        Public Property Item As SampleOrder
            Get
                Return _item
            End Get
            Set(value As SampleOrder)
                _item = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Item)))
            End Set
        End Property

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            Item = e.Parameter
        End Sub

        Private Sub WindowStates_CurrentStateChanged(sender As Object, e As VisualStateChangedEventArgs)
            If e.OldState = NarrowState AndAlso e.NewState = WideState Then
                NavigationService.GoBack()
            End If
        End Sub
    End Class
End Namespace
