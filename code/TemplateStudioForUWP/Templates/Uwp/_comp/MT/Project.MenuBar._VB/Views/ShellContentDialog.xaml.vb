Imports System
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Animation
Imports Param_RootNamespace.ViewModels

Namespace Views
    Public NotInheritable Partial Class ShellContentDialog
        Inherits ContentDialog

        Public ReadOnly Property ViewModel As ShellContentDialogViewModel = New ShellContentDialogViewModel()

        Private Sub New(pageType As Type, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing)
            InitializeComponent()
            shellFrame.Navigate(pageType, parameter, infoOverride)
            shellFrame.Width = Window.Current.Bounds.Width * 0.8
            shellFrame.Height = Window.Current.Bounds.Height * 0.8
        End Sub

        Public Shared Function Create(pageType As Type, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As ShellContentDialog
            Return New ShellContentDialog(pageType, parameter, infoOverride)
        End Function
    End Class
End Namespace
