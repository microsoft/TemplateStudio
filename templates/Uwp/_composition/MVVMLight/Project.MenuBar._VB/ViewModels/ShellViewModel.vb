Imports System.Windows.Input
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace ViewModels
    Public Class ShellViewModel
        Inherits ViewModelBase

        Private _menuFileExitCommand As ICommand

        Public ReadOnly Property MenuFileExitCommand As ICommand
            Get
                If _menuFileExitCommand Is Nothing Then
                    _menuFileExitCommand = New RelayCommand(AddressOf OnMenuFileExit)
                End If

                Return _menuFileExitCommand
            End Get
        End Property

        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Sub Initialize(shellFrame As Frame, splitView As SplitView, rightFrame As Frame)
            NavigationService.Frame = shellFrame
            MenuNavigationHelper.Initialize(splitView, rightFrame)
        End Sub

        Private Sub OnMenuFileExit()
            Application.Current.[Exit]()
        End Sub
    End Class
End Namespace
