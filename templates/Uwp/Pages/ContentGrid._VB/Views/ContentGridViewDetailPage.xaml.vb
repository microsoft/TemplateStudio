Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace Views
    Public NotInheritable Partial Class ContentGridViewDetailPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Dim orderId As Long
            orderId = CType(e.Parameter, Long)
            Await ViewModel.InitializeAsync(orderId)
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            If e.NavigationMode = NavigationMode.Back Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Item)
            End If
        End Sub
    End Class
End Namespace
