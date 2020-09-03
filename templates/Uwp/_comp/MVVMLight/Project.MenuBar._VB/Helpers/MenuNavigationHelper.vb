Imports Param_RootNamespace.Services
Imports Param_RootNamespace.ViewModels
Imports Param_RootNamespace.Views
Imports Windows.UI.Xaml.Media.Animation

Namespace Helpers
    Module MenuNavigationHelper
        Private _lastParamUsed As Object
        Private _splitView As SplitView
        Private _rightFrame As Frame

        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property

        Sub Initialize(splitView As SplitView, rightFrame As Frame)
            _splitView = splitView
            _rightFrame = rightFrame
        End Sub

        Sub UpdateView(pageKey As String, Optional parameters As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing)
            NavigationService.Navigate(pageKey, parameters, infoOverride, True)
        End Sub

        Sub Navigate(pageKey As String, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing)
            NavigationService.Navigate(pageKey, parameter, infoOverride)
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
