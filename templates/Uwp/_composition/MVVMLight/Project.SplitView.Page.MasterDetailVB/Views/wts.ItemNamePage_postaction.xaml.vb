'{[{
Imports Windows.UI.Xaml.Navigation
'}]}

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

'{[{

        Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
            MyBase.OnNavigatedFrom(e)

            ' Workaround for issue on MasterDetail Control. Find More info at https://github.com/Microsoft/WindowsTemplateStudio/issues/2738
            ViewModel.Selected = Nothing
        End Sub
'}]}
    End Class
End Namespace