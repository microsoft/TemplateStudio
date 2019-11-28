Imports Windows.UI.Core
Imports Param_RootNamespace.Helpers

Namespace Services

    ' A custom event that fires whenever the secondary view is ready to be closed. You should
    ' clean up any state (including deregistering for events) then close the window in this handler
    Public Delegate Sub ViewReleasedHandler(sender As Object, e As EventArgs)

    ' Whenever the main view is about to interact with the secondary view, it should call
    ' StartViewInUse on this object. When finished interacting, it should call StopViewInUse.
    Public NotInheritable Class ViewLifetimeControl

        Private ReadOnly _lockObj As Object = New Object()

        ' Window for this particular view. Used to register and unregister for events
        Private _window As CoreWindow

        Private _refCount As Integer = 0

        Private _released As Boolean = False

        Private Event InternalReleased As ViewReleasedHandler

        ' Necessary to communicate with the window
        Public Property Dispatcher As CoreDispatcher

        ' This id is used in all of the ApplicationViewSwitcher and ProjectionManager APIs
        Public Property Id As Integer

        Public Property Title As String

        Public Custom Event Released As ViewReleasedHandler
            AddHandler(value As ViewReleasedHandler)
                Dim releasedCopy As Boolean = False
                SyncLock _lockObj
                    releasedCopy = _released
                    If Not _released Then
                        AddHandler InternalReleased, value
                    End If

                End SyncLock

                If releasedCopy Then
                    Throw New InvalidOperationException("ExceptionViewLifeTimeControlViewDisposal".GetLocalized())
                End If
            End AddHandler

            RemoveHandler(value As ViewReleasedHandler)
                SyncLock _lockObj
                    RemoveHandler InternalReleased, value
                End SyncLock
            End RemoveHandler

            RaiseEvent(sender As Object, e As System.EventArgs)
                RaiseEvent InternalReleased(sender, e)
            End RaiseEvent
        End Event

        Private Sub New(newWindow As CoreWindow)
            Dispatcher = newWindow.Dispatcher
            _window = newWindow
            Id = ApplicationView.GetApplicationViewIdForWindow(_window)
            RegisterForEvents()
        End Sub

        Public Shared Function CreateForCurrentView() As ViewLifetimeControl
            Return New ViewLifetimeControl(CoreWindow.GetForCurrentThread())
        End Function

        ' Signals that the view is being interacted with by another view,
        ' so it shouldn't be closed even if it becomes "consolidated"
        Public Function StartViewInUse() As Integer
            Dim releasedCopy As Boolean = False
            Dim refCountCopy As Integer = 0
            SyncLock _lockObj
                releasedCopy = _released
                If Not _released Then
                    refCountCopy = System.Threading.Interlocked.Increment(_refCount)
                End If

            End SyncLock

            If releasedCopy Then
                Throw New InvalidOperationException("ExceptionViewLifeTimeControlViewDisposal".GetLocalized())
            End If

            Return refCountCopy
        End Function

        ' Should come after any call to StartViewInUse
        ' Signals that the another view has finished interacting with the view tracked by this object
        Public Function StopViewInUse() As Integer
            Dim refCountCopy As Integer = 0
            Dim releasedCopy As Boolean = False
            SyncLock _lockObj
                releasedCopy = _released
                If Not _released Then
                    refCountCopy = System.Threading.Interlocked.Decrement(_refCount)
                    If refCountCopy = 0 Then
                        Dispatcher.RunAsync(CoreDispatcherPriority.Low, AddressOf FinalizeRelease).AsTask()
                    End If
                End If

            End SyncLock

            If releasedCopy Then
                Throw New InvalidOperationException("ExceptionViewLifeTimeControlViewDisposal".GetLocalized())
            End If

            Return refCountCopy
        End Function

        Private Sub RegisterForEvents()
           AddHandler ApplicationView.GetForCurrentView().Consolidated, AddressOf ViewConsolidated
        End Sub

        Private Sub UnregisterForEvents()
            RemoveHandler ApplicationView.GetForCurrentView().Consolidated, AddressOf ViewConsolidated
        End Sub

        Private Sub ViewConsolidated(sender As ApplicationView, e As ApplicationViewConsolidatedEventArgs)
            StopViewInUse()
        End Sub

        Private Sub FinalizeRelease()
            Dim justReleased As Boolean = False
            SyncLock _lockObj
                If _refCount = 0 Then
                    justReleased = True
                    _released = True
                End If

            End SyncLock

            If justReleased Then
                UnregisterForEvents()

                If InternalReleasedEvent Is Nothing Then
                    ' For more information about using Multiple Views, see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/features/multiple-views.md
                    Throw New InvalidOperationException("ExceptionViewLifeTimeControlMissingReleasedSubscription".GetLocalized())
                End If

                RaiseEvent InternalReleased(Me, Nothing)
            End If
        End Sub
    End Class
End Namespace
