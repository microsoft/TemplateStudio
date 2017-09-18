Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports Param_ItemNamespace.Models

Namespace Views
    Partial NotInheritable Class MasterDetailDetailPage
        Inherits Page

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ViewModel.Item = e.Parameter
        End Sub
    End Class
End Namespace
