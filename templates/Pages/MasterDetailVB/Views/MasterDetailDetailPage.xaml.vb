Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports Param_ItemNamespace.Models

Namespace Views
    Public NotInheritable Partial Class MasterDetailDetailPage
        Inherits Page
        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ViewModel.Item = TryCast(e.Parameter, SampleOrder)
        End Sub
    End Class
End Namespace
