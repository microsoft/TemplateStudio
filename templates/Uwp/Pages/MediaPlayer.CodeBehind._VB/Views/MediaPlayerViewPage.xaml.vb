Imports Windows.Media.Core
Imports Windows.Media.Playback
Imports Windows.System.Display
Imports Windows.UI.Core

Namespace Views
    Public NotInheritable Partial Class MediaPlayerViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        ' TODO WTS: Set your default video and image URIs
        ' For more on the MediaPlayer and adjusting controls and behavior see https://docs.microsoft.com/windows/uwp/controls-and-patterns/media-playback
        Private Const DefaultSource As String = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_high.mp4"

        ' The poster image is displayed until the video is started
        Private Const DefaultPoster As String = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_960.jpg"

        ' The DisplayRequest is used to stop the screen dimming while watching for extended periods
        Private _displayRequest As New DisplayRequest()

        Private _isRequestActive As Boolean = False

        Public Sub New()
            InitializeComponent()

            mpe.PosterSource = New BitmapImage(New Uri(DefaultPoster))
            mpe.Source = MediaSource.CreateFromUri(New Uri(DefaultSource))
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            AddHandler mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged, AddressOf PlaybackSession_PlaybackStateChanged
        End Sub

        Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
            MyBase.OnNavigatedFrom(e)
            mpe.MediaPlayer.Pause()
            RemoveHandler mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged, AddressOf PlaybackSession_PlaybackStateChanged
            Dim mediaSource = TryCast(mpe.Source, MediaSource)
            mediaSource?.Dispose()
            mpe.Source = Nothing
        End Sub

        Private Async Sub PlaybackSession_PlaybackStateChanged(sender As MediaPlaybackSession, args As Object)
            Dim playbackSession = TryCast(sender, MediaPlaybackSession)

            If playbackSession IsNot Nothing AndAlso playbackSession.NaturalVideoHeight <> 0 Then
                If playbackSession.PlaybackState = MediaPlaybackState.Playing Then
                    If Not _isRequestActive Then
                        Await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            Sub()
                                _displayRequest.RequestActive()
                                _isRequestActive = True
                            End Sub)
                    End If
                Else
                    If _isRequestActive Then
                        Await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                         Sub()
                            _displayRequest.RequestRelease()
                            _isRequestActive = False
                        End Sub)
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
