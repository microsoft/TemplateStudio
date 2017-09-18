Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Param_ItemNamespace.Models

Namespace Views
    Partial NotInheritable Class MasterDetailDetailControl
        Inherits UserControl

        Public Property MasterMenuItem As SampleOrder
            Get
                Return GetValue(MasterMenuItemProperty)
            End Get
            Set
                SetValue(MasterMenuItemProperty, Value)
            End Set
        End Property
        Public Shared ReadOnly MasterMenuItemProperty As DependencyProperty =
                               DependencyProperty.Register(NameOf(MasterMenuItem),
                               GetType(SampleOrder), GetType(MasterDetailDetailControl),
                               New PropertyMetadata(Nothing))

    End Class
End Namespace
