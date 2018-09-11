'{[{
Imports System.Linq
Imports Param_ItemNamespace.Activation
Imports Param_ItemNamespace.Helpers
'}]}
Namespace Views
    Public NotInheritable Partial Class PivotPage
        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
'{[{
            Dim data = TryCast(e.Parameter, SchemeActivationData)
            If data IsNot Nothing
                Await InitializeFromSchemeActivationAsync(data)
            End If

'}]}
            Await Task.CompletedTask
        End Sub

'{[{
        Public Async Function InitializeFromSchemeActivationAsync(schemeActivationData As SchemeActivationData) As Task
            Dim selected = pivot.Items.Cast(Of PivotItem)().FirstOrDefault(Function(i) i.IsOfPageType(schemeActivationData.PageType))
            Dim page = selected?.GetPage(Of IPivotActivationPage)()
            pivot.SelectedItem = selected
            Await page?.OnPivotActivatedAsync(schemeActivationData.Parameters)
        End Function
'}]}
    End Class
End Namespace
