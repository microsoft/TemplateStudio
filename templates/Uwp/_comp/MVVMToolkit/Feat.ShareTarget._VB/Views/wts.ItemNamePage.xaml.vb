Imports Windows.ApplicationModel.DataTransfer.ShareTarget
Imports Param_RootNamespace.ViewModels

Namespace Views
    ' TODO WTS: Remove this example page when/if it's not needed.
    ' This page is an example of how to handle data that is shared with your app.
    ' You can either change this page to meet your needs, or use another and delete this page.
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public ReadOnly Property ViewModel As wts.ItemNameViewModel = new wts.ItemNameViewModel

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Await ViewModel.LoadAsync(TryCast(e.Parameter, ShareOperation))
        End Sub
    End Class
End Namespace
