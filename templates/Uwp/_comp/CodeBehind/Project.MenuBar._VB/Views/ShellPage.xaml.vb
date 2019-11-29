Imports System
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Windows.System
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Input
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services

Namespace Views
    ' TODO WTS: You can edit the text for the menu in String/en-US/Resources.resw
    ' You can show pages in different ways (update main view, navigate, right pane, new windows or dialog) using MenuNavigationHelper class.
    ' Read more about MenuBar project type here:
    ' https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/projectTypes/menubar.md
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private ReadOnly _altLeftKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu)
        Private ReadOnly _backKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack)

        Public Sub New()
            InitializeComponent()
            NavigationService.Frame = shellFrame
            MenuNavigationHelper.Initialize(splitView, rightFrame)
        End Sub

        Private Async Sub OnLoaded(sender As Object, e As RoutedEventArgs)
            ' Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            ' More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            keyboardAccelerators.Add(_altLeftKeyboardAccelerator)
            keyboardAccelerators.Add(_backKeyboardAccelerator)
            Await Task.CompletedTask
        End Sub

        Private Sub ShellMenuItemClick_File_Exit(sender As Object, e As RoutedEventArgs)
            Application.Current.[Exit]()
        End Sub

        Private Function BuildKeyboardAccelerator(key As VirtualKey, Optional modifiers As VirtualKeyModifiers? = Nothing) As KeyboardAccelerator
            Dim keyboardAccelerator = New KeyboardAccelerator() With {
                .Key = key
            }

            If modifiers.HasValue Then
                keyboardAccelerator.Modifiers = modifiers.Value
            End If

            AddHandler keyboardAccelerator.Invoked, AddressOf OnKeyboardAcceleratorInvoked
            Return keyboardAccelerator
        End Function

        Private Overloads Sub OnKeyboardAcceleratorInvoked(sender As KeyboardAccelerator, args As KeyboardAcceleratorInvokedEventArgs)
            Dim result = NavigationService.GoBack()
            args.Handled = result
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
