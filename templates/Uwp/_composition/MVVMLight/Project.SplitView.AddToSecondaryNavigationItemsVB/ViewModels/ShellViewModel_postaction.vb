'{**
'This code block adds the wts.ItemNameViewModel to the _secondaryItems of the ShellViewModel.
'**}

'{[{
Imports Param_ItemNamespace.Helpers
'}]}
Namespace ViewModels
    Public Class ShellViewModel
        Inherits ViewModelBase

        Private Sub PopulateNavItems()
            '^^
            '{[{
            _secondaryItems.Add(New ShellNavigationItem("Shell_wts.ItemName".GetLocalized(), Symbol.Setting, GetType(wts.ItemNameViewModel).FullName))
            '}]}
        End Sub
    End Class
End Namespace
