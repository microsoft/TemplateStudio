Imports wts.ItemName.Services
Imports wts.ItemName.ViewModels

Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page

        Private ReadOnly Property ViewModel() As ShellViewModel
            Get
                Return TryCast(DataContext, ShellViewModel)
            End Get
        End Property

        Public Sub New()
            Me.InitializeComponent()
            DataContext = ViewModel
            ViewModel.Initialize(shellFrame)
        End Sub
    End Class
End Namespace
