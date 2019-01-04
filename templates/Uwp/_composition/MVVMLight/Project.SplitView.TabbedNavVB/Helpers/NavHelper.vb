Imports Microsoft.UI.Xaml.Controls

Namespace Helpers
    Public Class NavHelper

        Public Shared Function GetNavigateTo(item As NavigationViewItem) As String
            Return TryCast(item.GetValue(NavigateToProperty), String)
        End Function

        Public Shared Sub SetNavigateTo(item As NavigationViewItem, value As String)
            item.SetValue(NavigateToProperty, value)
        End Sub

        Public Shared ReadOnly NavigateToProperty As DependencyProperty =
           DependencyProperty.RegisterAttached("NavigateTo", GetType(String), GetType(NavHelper), New PropertyMetadata(Nothing))
    End Class
End Namespace
