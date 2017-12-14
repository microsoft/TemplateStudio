Imports Windows.ApplicationModel.DataTransfer.ShareTarget
Imports Param_ItemNamespace.ViewModels

Namespace Views
    ' TODO WTS: This page exists purely as an example of how to launch a specific page
    ' in response to a protocol launch and pass it a value. It is expected that you will
    ' delete this page once you have changed the handling of a protocol launch to meet your
    ' needs and redirected to another of your pages.
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public ReadOnly Property ViewModel() As wts.ItemNameViewModel = new wts.ItemNameViewModel

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Await ViewModel.LoadAsync(TryCast(e.Parameter, ShareOperation))
        End Sub
    End Class
End Namespace
