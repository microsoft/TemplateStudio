Imports Windows.Media.Playback
Imports Windows.Media.Core

Namespace ViewModels
    Public Class MediaPlayerViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        ' TODO WTS: Specify your video default and image here
        Private Const DefaultSource As String = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_high.mp4"

        ' The poster image is displayed until the video is started
        Private Const DefaultPoster As String = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_960.jpg"

        Private _source As IMediaPlaybackSource

        Public Property Source As IMediaPlaybackSource
            Get
                Return _source
            End Get
            Set
                [Param_Setter](_source, value)
            End Set
        End Property

        Private _posterSource As String

        Public Property PosterSource As String
            Get
                Return _posterSource
            End Get
            Set
                [Param_Setter](_posterSource, newValue := value)
            End Set
        End Property

        Public Sub New()
            Source = MediaSource.CreateFromUri(New Uri(DefaultSource))
            PosterSource = DefaultPoster
        End Sub
    End Class
End Namespace
