Imports Param_RootNamespace.Services
Imports Param_RootNamespace.Views
Imports System
Imports System.Threading.Tasks
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Animation

Namespace Helpers
    Module MenuNavigationHelper
        Private _lastParamUsed As Object
        Private _splitView As SplitView
        Private _rightFrame As Frame

        Sub Initialize(splitView As SplitView, rightFrame As Frame)
            _splitView = splitView
            _rightFrame = rightFrame
        End Sub

        Sub UpdateView(pageType As Type, Optional parameters As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing)
            NavigationService.Navigate(pageType, parameters, infoOverride, True)
        End Sub

        Sub Navigate(pageType As Type, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing)
            NavigationService.Navigate(pageType, parameter, infoOverride)
        End Sub

        Sub OpenInRightPane(pageType As Type, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing)
            ' Don't open the same page multiple times
            If _rightFrame.Content?.[GetType]() <> pageType OrElse (parameter IsNot Nothing AndAlso Not parameter.Equals(_lastParamUsed)) Then
                Dim navigationResult = _rightFrame.Navigate(pageType, parameter, infoOverride)

                If navigationResult Then
                    _lastParamUsed = parameter
                End If
            End If

            _splitView.IsPaneOpen = True
        End Sub

        Async Function OpenInNewWindow(pageType As Type) As Task
            Await WindowManagerService.Current.TryShowAsStandaloneAsync(pageType.Name, pageType)
        End Function

        Async Function OpenInDialog(pageType As Type, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Task
            Dim dialog = ShellContentDialog.Create(pageType, parameter, infoOverride)
            Await dialog.ShowAsync()
        End Function
    End Module
End Namespace
