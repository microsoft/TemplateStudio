'{[{
Imports Param_RootNamespace.Services
'}]}

Namespace Behaviors
    Public Class NavigationViewHeaderBehavior
        Inherits Behavior(Of WinUI.NavigationView)

        Protected Overrides Sub OnAttached()
'^^
'{[{
            AddHandler NavigationService.Navigated, AddressOf OnNavigated
'}]}
        End Sub

        Protected Overrides Sub OnDetaching()
'^^
'{[{
            RemoveHandler NavigationService.Navigated, AddressOf OnNavigated
'}]}
        End Sub
    End Class
End Namespace
