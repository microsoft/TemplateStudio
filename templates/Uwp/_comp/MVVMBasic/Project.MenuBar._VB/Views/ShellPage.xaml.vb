Imports System
Imports Windows.UI.Xaml.Controls
Imports Param_RootNamespace.ViewModels

Namespace Views
    ' TODO WTS: You can edit the text for the menu in String/en-US/Resources.resw
    ' You can show pages in different ways (update main view, navigate, right pane, new windows or dialog) using MenuNavigationHelper class.
    ' Read more about MenuBar project type here:
    ' https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/projectTypes/menubar.md
    Public NotInheritable Partial Class ShellPage
        Inherits Page

        Public ReadOnly Property ViewModel As ShellViewModel = New ShellViewModel()

        Public Sub New()
            InitializeComponent()
            ViewModel.Initialize(shellFrame, splitView, rightFrame, KeyboardAccelerators)
        End Sub
    End Class
End Namespace
