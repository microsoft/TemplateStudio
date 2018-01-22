Imports Windows.Media.Playback
Imports Windows.System.Display
Imports Windows.UI.Core

Namespace Views
    Public NotInheritable Partial Class MediaPlayerViewPage
        Inherits Page

        ' For more on the MediaPlayer and adjusting controls and behavior see https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/media-playback
        ' The DisplayRequest is used to stop the screen dimming while watching for extended periods
        Private _displayRequest As New DisplayRequest()

        Private _isRequestActive As Boolean = False

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Async Sub PlaybackSession_PlaybackStateChanged(sender As MediaPlaybackSession, args As Object)
            Dim playbackSession = TryCast(sender, MediaPlaybackSession)

            If playbackSession IsNot Nothing AndAlso playbackSession.NaturalVideoHeight <> 0 Then
                If playbackSession.PlaybackState = MediaPlaybackState.Playing Then
                    If Not _isRequestActive Then
                        Await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub() 
                            _displayRequest.RequestActive()
                            _isRequestActive = True
                        End Sub)
                    End If
                Else
                    If _isRequestActive Then
                        Await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub() 
                            _displayRequest.RequestRelease()
                            _isRequestActive = False
                        End Sub)
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
