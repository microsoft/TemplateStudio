Imports Microsoft.UI.Xaml.Controls

Namespace Helpers
    Public Class NavHelper
        ' This helper class allows to specify the page that will be shown when you click on a NavigationViewItem
        '
        ' Usage in xaml:
        ' <winui:NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavHelper.NavigateTo="views:MainPage" />
        '
        ' Usage in code:
        ' NavHelper.SetNavigateTo(navigationViewItem, GetType(MainPage))
        Public Shared Function GetNavigateTo(item As NavigationViewItem) As Type
            Return CType(item.GetValue(NavigateToProperty), Type)
        End Function

        Public Shared Sub SetNavigateTo(item As NavigationViewItem, value As Type)
            item.SetValue(NavigateToProperty, value)
        End Sub

        Public Shared ReadOnly NavigateToProperty As DependencyProperty =
           DependencyProperty.RegisterAttached("NavigateTo", GetType(Type), GetType(NavHelper), New PropertyMetadata(Nothing))
    End Class
End Namespace
