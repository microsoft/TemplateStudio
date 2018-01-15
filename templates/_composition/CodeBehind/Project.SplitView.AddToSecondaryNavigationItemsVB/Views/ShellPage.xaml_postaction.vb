'{**
'This code block adds the wts.ItemNamePage to the _secondaryItems of the ShellPage.
'**}

Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private Sub PopulateNavItems()
            '^^
            '{[{
            _secondaryItems.Add(ShellNavigationItem.FromType(Of wts.ItemNamePage)("Shell_wts.ItemName".GetLocalized(), Symbol.Setting))
            '}]}
        End Sub
    End Class
End Namespace
