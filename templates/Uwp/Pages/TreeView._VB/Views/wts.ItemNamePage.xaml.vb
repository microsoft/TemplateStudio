Namespace Views
    ' For more info about the TreeView Control see
    ' https://docs.microsoft.com/windows/uwp/design/controls-and-patterns/tree-view
    ' For other samples, get the XAML Controls Gallery app http://aka.ms/XamlControlsGallery
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Await ViewModel.LoadDataAsync()
        End Sub
    End Class
End Namespace