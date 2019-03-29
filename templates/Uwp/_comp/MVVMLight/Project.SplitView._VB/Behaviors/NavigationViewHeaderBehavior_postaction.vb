'{[{
Imports Param_RootNamespace.ViewModels
'}]}

Namespace Behaviors
    Public Class NavigationViewHeaderBehavior
        Inherits Behavior(Of WinUI.NavigationView)

        Protected Overrides Sub OnAttached()
            '^^
            '{[{
            AddHandler ViewModelLocator.Current.NavigationService.Navigated, AddressOf OnNavigated
            '}]}
        End Sub
    End Class
End Namespace