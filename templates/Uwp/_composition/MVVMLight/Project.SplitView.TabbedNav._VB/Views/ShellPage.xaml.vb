Imports Param_RootNamespace.ViewModels

Namespace Views
    ' TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    Public NotInheritable Partial Class ShellPage
        Inherits Page

        Private ReadOnly Property ViewModel As ShellViewModel
            Get
                Return ViewModelLocator.Current.ShellViewModel
            End Get
        End Property

        Public Sub New()
            Me.InitializeComponent()
            DataContext = ViewModel
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators)
        End Sub

    End Class
End Namespace
