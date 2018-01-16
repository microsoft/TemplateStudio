Imports wts.ItemName.ViewModels

Namespace Views
    Public NotInheritable Partial Class PivotPage
        Inherits Page

        Private ReadOnly Property ViewModel As PivotViewModel
            Get
                Return TryCast(DataContext, PivotViewModel)
            End Get
        End Property

        Public Sub New()
            Me.InitializeComponent()
            ' We use NavigationCacheMode.Required to keep track the selected item on navigation. For further information see the following links.
            ' https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.ui.xaml.controls.page.navigationcachemode.aspx
            ' https://msdn.microsoft.com/en-us/library/windows/apps/xaml/Hh771188.aspx
            NavigationCacheMode = NavigationCacheMode.Required
        End Sub
    End Class
End Namespace
