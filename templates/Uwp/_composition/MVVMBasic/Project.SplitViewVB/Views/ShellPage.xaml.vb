Imports wts.ItemName.ViewModels
Imports wts.ItemName.Services
Imports Windows.Foundation.Metadata

Namespace Views
    ' TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    Public NotInheritable Partial Class ShellPage
      Inherits Page

        Public ReadOnly Property ViewModel As New ShellViewModel

        Public Sub New()
            Me.InitializeComponent()
            HideNavViewBackButton()
            DataContext = ViewModel
            ViewModel.Initialize(shellFrame, navigationView)
            KeyboardAccelerators.Add(ActivationService.AltLeftKeyboardAccelerator)
            KeyboardAccelerators.Add(ActivationService.BackKeyboardAccelerator)
        End Sub

        Private Sub HideNavViewBackButton()
            If ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6) Then
                navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed
            End if
        End Sub
    End Class
End Namespace
