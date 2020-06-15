Imports Param_RootNamespace.Core.Models
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace Views
    Public NotInheritable Partial Class wts.ItemNameListItemControl
        Inherits UserControl

        Public Property Item As SampleOrder
            Get
                Return CType(GetValue(ItemProperty), SampleOrder)
            End Get
            Set(value As SampleOrder)
                SetValue(ItemProperty, value)
            End Set
        End Property

        Public Shared ReadOnly ItemProperty As DependencyProperty = DependencyProperty.Register(NameOf(Item), GetType(SampleOrder), GetType(wts.ItemNameListItemControl), New PropertyMetadata(Nothing, AddressOf OnItemPropertyChanged))

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Shared Sub OnItemPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        End Sub
    End Class
End Namespace
