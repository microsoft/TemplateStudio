Imports Param_RootNamespace.Core.Models
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace Views
    Public NotInheritable Partial Class wts.ItemNameDetailControl
        Inherits UserControl

        Public Property SelectedItem As SampleOrder
            Get
                Return TryCast(GetValue(SelectedItemProperty), SampleOrder)
            End Get
            Set(value As SampleOrder)
                SetValue(SelectedItemProperty, value)
            End Set
        End Property

        Public Shared ReadOnly SelectedItemProperty As DependencyProperty = DependencyProperty.Register(NameOf(SelectedItem), GetType(SampleOrder), GetType(wts.ItemNameDetailControl), New PropertyMetadata(Nothing, AddressOf OnSelectedItemPropertyChanged))

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Shared Sub OnSelectedItemPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim control = TryCast(d, wts.ItemNameDetailControl)
            control.ForegroundElement.ChangeView(0, 0, 1)
        End Sub
    End Class
End Namespace
