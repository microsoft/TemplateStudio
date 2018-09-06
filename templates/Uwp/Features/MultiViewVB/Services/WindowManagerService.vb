Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core

Namespace Services

    Public Delegate Sub ViewClosedHandler(viewControl As ViewLifetimeControl, e As EventArgs)

    ' For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/multiple-views.md
    ' More details about showing multiple views at https://docs.microsoft.com/windows/uwp/design/layout/show-multiple-views
    Public Class WindowManagerService

        Private Shared _current As WindowManagerService

        Public Shared ReadOnly Property Current As WindowManagerService
            Get
                If _current Is Nothing Then
                    _current = New WindowManagerService()
                End If

                Return _current
            End Get
        End Property

        ' Contains all the opened secondary views.
        Public Property SecondaryViews As ObservableCollection(Of ViewLifetimeControl) = New ObservableCollection(Of ViewLifetimeControl)()

        Public Property MainViewId As Integer

        Public Property MainDispatcher As CoreDispatcher

        Public Sub Initialize()
            MainViewId = ApplicationView.GetForCurrentView().Id
            MainDispatcher = Window.Current.Dispatcher
        End Sub

        ' Displays a view as a standalone
        ' You can use the resulting ViewLifeTileControl to interact with the new window.
        Public Async Function TryShowAsStandaloneAsync(windowTitle As String, pageType As Type) As Task(Of ViewLifetimeControl)
            Dim theme = (TryCast(Window.Current.Content, FrameworkElement)).RequestedTheme
            Dim viewControl As ViewLifetimeControl = Await CreateViewLifetimeControlAsync(windowTitle, pageType, theme)
            SecondaryViews.Add(viewControl)
            viewControl.StartViewInUse()
            Dim viewShown = Await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewControl.Id, ViewSizePreference.[Default], ApplicationView.GetForCurrentView().Id, ViewSizePreference.[Default])
            viewControl.StopViewInUse()
            Return viewControl
        End Function

        ' Displays a view in the specified view mode
        Public Async Function TryShowAsViewModeAsync(windowTitle As String, pageType As Type, Optional viewMode As ApplicationViewMode = ApplicationViewMode.[Default]) As Task(Of ViewLifetimeControl)
            Dim theme = (TryCast(Window.Current.Content, FrameworkElement)).RequestedTheme
            Dim viewControl As ViewLifetimeControl = Await CreateViewLifetimeControlAsync(windowTitle, pageType, theme)
            SecondaryViews.Add(viewControl)
            viewControl.StartViewInUse()
            Dim viewShown = Await ApplicationViewSwitcher.TryShowAsViewModeAsync(viewControl.Id, viewMode)
            viewControl.StopViewInUse()
            Return viewControl
        End Function

        Private Async Function CreateViewLifetimeControlAsync(windowTitle As String, pageType As Type, theme As ElementTheme) As Task(Of ViewLifetimeControl)
            Dim viewControl As ViewLifetimeControl = Nothing
            Await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                viewControl = ViewLifetimeControl.CreateForCurrentView()
                viewControl.Title = windowTitle
                viewControl.StartViewInUse()
                Dim frame = New Frame() With {
                    .RequestedTheme = theme
                }
                frame.Navigate(pageType, viewControl)
                Window.Current.Content = frame
                Window.Current.Activate()
                ApplicationView.GetForCurrentView().Title = viewControl.Title
            End Sub)
            Return viewControl
        End Function

        Public Function IsWindowOpen(windowTitle As String) As Boolean
            Return SecondaryViews.Any(Function(v) v.Title = windowTitle)
        End Function
    End Class
End Namespace
