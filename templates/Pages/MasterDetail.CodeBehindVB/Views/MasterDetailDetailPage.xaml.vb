Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services

Namespace Views
    Public NotInheritable Partial Class MasterDetailDetailPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

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
            InitializeComponent()
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            Item = TryCast(e.Parameter, SampleOrder)
        End Sub

        Private Sub WindowStates_CurrentStateChanged(sender As Object, e As VisualStateChangedEventArgs)
            If e.OldState.Equals(NarrowState) AndAlso e.NewState.Equals(WideState) Then
                NavigationService.GoBack()
            End If
        End Sub
    End Class
End Namespace

