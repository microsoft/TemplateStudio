Imports wts.ItemName.ViewModels
Imports WinUI = Microsoft.UI.Xaml.Controls

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

        Private Sub OnItemInvoked(sender As WinUI.NavigationView, args As WinUI.NavigationViewItemInvokedEventArgs)
            ' Workaround for Issue https://github.com/Microsoft/WindowsTemplateStudio/issues/2774
            ' Using EventTriggerBehavior does not work on WinUI NavigationView ItemInvoked event in Release mode.
            ViewModel.ItemInvokedCommand.Execute(args)
        End Sub

    End Class
End Namespace
