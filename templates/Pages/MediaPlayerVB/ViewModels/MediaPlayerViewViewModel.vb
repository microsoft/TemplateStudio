Imports System.Collections.ObjectModel
Imports Windows.Media.Playback
Imports Windows.Media.Core
Imports System.ComponentModel

Namespace ViewModels
    Public Class MediaPlayerViewViewModel
        Implements INotifyPropertyChanged

        ' TODO WTS: Set your video default and image here
        Private Const DefaultSource As String = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_high.mp4"

        ' The poster image is displayed until the video is started
        Private Const DefaultPoster As String = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_960.jpg"

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _source As IMediaPlaybackSource

        Public Property Source As IMediaPlaybackSource
            Get
                Return _source
            End Get
            Set(value As IMediaPlaybackSource)
                _source = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Source)))
            End Set
        End Property

        Private _posterSource As String

        Public Property PosterSource As String
            Get
                Return _posterSource
            End Get
            Set(value As String)
                _posterSource = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(PosterSource)))
            End Set
        End Property

        Public Sub New()
            Source = MediaSource.CreateFromUri(New Uri(DefaultSource))
            PosterSource = DefaultPoster
        End Sub
    End Class
End Namespace
