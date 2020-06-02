Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            DataContext = ViewModel
            ViewModel.Initialize(twoPaneView)
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Await ViewModel.LoadDataAsync()
        End Sub
    End Class
End Namespace
