Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Windows.ApplicationModel.Activation
Imports Windows.UI.Core
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation

Imports wts.DefaultProject.Activation

Namespace Services
    'For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    Friend Class ActivationService
        Private ReadOnly _app As App
        Private ReadOnly _shell As UIElement
        Private ReadOnly _defaultNavItem As Type

        Public Sub New(app As App, defaultNavItem As Type, Optional shell As UIElement = Nothing)
            _app = app
            _shell = If(shell, New Frame())
            _defaultNavItem = defaultNavItem
        End Sub

        Public Async Function ActivateAsync(activationArgs As Object) As Task
            If IsInteractive(activationArgs) Then
                ' Initialize things like registering background task before the app is loaded
                Await InitializeAsync()

                ' Do not repeat app initialization when the Window already has content,
                ' just ensure that the window is active
                If Window.Current.Content Is Nothing Then
                    ' Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = _shell
                    AddHandler NavigationService.Frame.NavigationFailed, Function(sender, e)
                    Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
                                                                End Function
                    AddHandler NavigationService.Navigated, AddressOf OnFrameNavigated
                    If SystemNavigationManager.GetForCurrentView() IsNot Nothing Then
                        AddHandler SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf OnAppViewBackButtonRequested
                    End If
                End If
            End If

            Dim activationHandler = GetActivationHandlers().FirstOrDefault(Function(h) h.CanHandle(activationArgs))

            If activationHandler IsNot Nothing Then
                Await activationHandler.HandleAsync(activationArgs)
            End If

            If IsInteractive(activationArgs) Then
                Dim defaultHandler = New DefaultLaunchActivationHandler(_defaultNavItem)
                If defaultHandler.CanHandle(activationArgs) Then
                    Await defaultHandler.HandleAsync(activationArgs)
                End If

                ' Ensure the current window is active
                Window.Current.Activate()

                ' Tasks after activation
                Await StartupAsync()
            End If
        End Function

        Private Async Function InitializeAsync() As Task
            Await Task.CompletedTask
        End Function

        Private Async Function StartupAsync() As Task
            Await Task.CompletedTask
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)

            Exit Function
        End Function

        Private Function IsInteractive(args As Object) As Boolean
            Return TypeOf args Is IActivatedEventArgs
        End Function

        Private Sub OnFrameNavigated(sender As Object, e As NavigationEventArgs)
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = If((NavigationService.CanGoBack), AppViewBackButtonVisibility.Visible, AppViewBackButtonVisibility.Collapsed)
        End Sub

        Private Sub OnAppViewBackButtonRequested(sender As Object, e As BackRequestedEventArgs)
            If NavigationService.CanGoBack Then
                NavigationService.GoBack()
                e.Handled = True
            End If
        End Sub
    End Class
End Namespace
