Imports Windows.UI.Xaml.Controls
Imports wts.ItemName.ViewModels

Namespace Views
    Public NotInheritable Partial Class ShellPage
      Inherits Page

        Public ReadOnly Property ViewModel() As ShellViewModel = New ShellViewModel

        Public Sub New()
            Me.InitializeComponent()
            DataContext = ViewModel
            ViewModel.Initialize(shellFrame)
        End Sub
    End Class
End Namespace
