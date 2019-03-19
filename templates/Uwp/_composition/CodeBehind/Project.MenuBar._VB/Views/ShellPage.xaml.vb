Imports System
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services

Namespace Views
    ' TODO WTS: You can edit the text for the menu in String/en-US/Resources.resw
    ' You can show pages in different ways (update main view, navigate, right pane, new windows or dialog) using MenuNavigationHelper class.
    ' Read more about MenuBar project type here:
    ' https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/menubar.md
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Sub New()
            InitializeComponent()
            NavigationService.Frame = shellFrame
            MenuNavigationHelper.Initialize(splitView, rightFrame)
        End Sub

        Private Sub OnLoaded(sender As Object, e As RoutedEventArgs)
            KeyboardAccelerators.Add(ActivationService.BackKeyboardAccelerator)
            KeyboardAccelerators.Add(ActivationService.AltLeftKeyboardAccelerator)
        End Sub

        Private Sub ShellMenuItemClick_File_Exit(sender As Object, e As RoutedEventArgs)
            Application.Current.[Exit]()
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
                Return
            End If

            storage = value
            OnPropertyChanged(propertyName)
        End Sub

        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
