Imports Param_ItemNamespace.Views
Namespace ViewModels
    Public Class ShellViewModel
        Inherits Observable
        Private Sub PopulateNavItems()
//^^
            _primaryItems.Add(ShellNavigationItem.FromType(Of wts.ItemNamePage)("Shell_wts.ItemName".GetLocalized(), Symbol.Document))
        End Sub
    End Class
End Namespace
