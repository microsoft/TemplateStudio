Imports System.Collections.Generic
Imports System.Windows.Input
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services
Imports Param_RootNamespace.Views
Imports Microsoft.Toolkit.Mvvm.ComponentModel
Imports Microsoft.Toolkit.Mvvm.Input
Imports Windows.System
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Input

Namespace ViewModels
    Public Class ShellViewModel
        Inherits ObservableObject

        Private ReadOnly _altLeftKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu)
        Private ReadOnly _backKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack)
        Private _keyboardAccelerators As IList(Of KeyboardAccelerator)

        Private _loadedCommand As ICommand
        Private _menuFileExitCommand As ICommand

        Public ReadOnly Property LoadedCommand As ICommand
            Get
                If _loadedCommand Is Nothing Then
                    _loadedCommand = New RelayCommand(AddressOf OnLoaded)
                End If

                Return _loadedCommand
            End Get
        End Property

        Public ReadOnly Property MenuFileExitCommand As ICommand
            Get
                If _menuFileExitCommand Is Nothing Then
                    _menuFileExitCommand = New RelayCommand(AddressOf OnMenuFileExit)
                End If

                Return _menuFileExitCommand
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Sub Initialize(shellFrame As Frame, splitView As SplitView, rightFrame As Frame, keyboardAccelerators As IList(Of KeyboardAccelerator))
            NavigationService.Frame = shellFrame
            MenuNavigationHelper.Initialize(splitView, rightFrame)
            _keyboardAccelerators = keyboardAccelerators
        End Sub

        Private Async Sub OnLoaded()
            ' Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            ' More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            _keyboardAccelerators.Add(_altLeftKeyboardAccelerator)
            _keyboardAccelerators.Add(_backKeyboardAccelerator)
            Await Task.CompletedTask
        End Sub

        Private Sub OnMenuFileExit()
            Application.Current.[Exit]()
        End Sub

        Private Shared Function BuildKeyboardAccelerator(key As VirtualKey, Optional modifiers As VirtualKeyModifiers? = Nothing) As KeyboardAccelerator
            Dim keyboardAccelerator = New KeyboardAccelerator() With {
                .Key = key
            }

            If modifiers.HasValue Then
                keyboardAccelerator.Modifiers = modifiers.Value
            End If

            AddHandler keyboardAccelerator.Invoked, AddressOf OnKeyboardAcceleratorInvoked
            Return keyboardAccelerator
        End Function

        Private Shared Sub OnKeyboardAcceleratorInvoked(sender As KeyboardAccelerator, args As KeyboardAcceleratorInvokedEventArgs)
            Dim result = NavigationService.GoBack()
            args.Handled = result
        End Sub
    End Class
End Namespace
