Imports Microsoft.UI.Xaml.Controls

Namespace Helpers
    Public Class NavHelper
        ' This helper class allows to specify the page that will be shown when you click on a NavigationViewItem
        '
        ' Usage in xaml:
        ' <winui:NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavHelper.NavigateTo="AppName.ViewModels.MainViewModel" />
        '
        ' Usage in code:
        ' NavHelper.SetNavigateTo(navigationViewItem, GetType(MainViewModel).FullName)
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
