//{[{
Imports Param_ItemNamespace.Helpers
//}]}
Namespace ViewModels
    Public Class ShellViewModel
        Inherits ViewModelBase
        Private Sub PopulateNavItems()
            //^^
            //{[{
            _primaryItems.Add(New ShellNavigationItem("Shell_wts.ItemName".GetLocalized(), Symbol.Document, GetType(wts.ItemNameViewModel).FullName))
            //}]}
        End Sub
    End Class
End Namespace
