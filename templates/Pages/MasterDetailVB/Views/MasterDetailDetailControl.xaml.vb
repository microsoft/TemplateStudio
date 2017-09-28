Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Param_ItemNamespace.Models

Namespace Views
    Public NotInheritable Partial Class MasterDetailDetailControl
        Inherits UserControl
        Public Property MasterMenuItem As SampleOrder
            Get
                Return TryCast(GetValue(MasterMenuItemProperty), SampleOrder)
            End Get
            Set
                SetValue(MasterMenuItemProperty, value)
            End Set
        End Property

        Public Shared ReadOnly MasterMenuItemProperty As DependencyProperty = DependencyProperty.Register("MasterMenuItem", GetType(SampleOrder), GetType(MasterDetailDetailControl), New PropertyMetadata(Nothing))

        Public Sub New()
            InitializeComponent()
        End Sub
    End Class
End Namespace
