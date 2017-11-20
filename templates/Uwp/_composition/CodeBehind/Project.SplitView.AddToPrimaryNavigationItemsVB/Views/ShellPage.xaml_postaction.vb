'{**
'This code block adds the wts.ItemNamePage to the _primaryItems of the ShellPage.
'**}

Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private Sub PopulateNavItems()
            '^^
            '{[{
            _primaryItems.Add(ShellNavigationItem.FromType(Of wts.ItemNamePage)("Shell_wts.ItemName".GetLocalized(), Symbol.Document))
            '}]}
        End Sub
    End Class
End Namespace
