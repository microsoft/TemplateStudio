'{**
'This code block adds the wts.ItemNamePage to the _primaryItems of the ShellViewModel.
'**}

'{[{
Imports Param_ItemNamespace.Views
'}]}
Namespace ViewModels
    Public Class ShellViewModel
        Inherits Observable

        Private Sub PopulateNavItems()
'^^
'{[{
            _primaryItems.Add(ShellNavigationItem.FromType(Of wts.ItemNamePage)("Shell_wts.ItemName".GetLocalized(), Symbol.Document))
'}]}
        End Sub
    End Class
End Namespace
