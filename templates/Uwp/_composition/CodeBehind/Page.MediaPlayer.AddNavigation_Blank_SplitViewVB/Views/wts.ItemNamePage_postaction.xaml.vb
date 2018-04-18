Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged

        '{[{

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            AddHandler mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged, AddressOf PlaybackSession_PlaybackStateChanged
        End Sub

        Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
            MyBase.OnNavigatedFrom(e)
            mpe.MediaPlayer.Pause()
            RemoveHandler mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged, AddressOf PlaybackSession_PlaybackStateChanged
        End Sub
        '}]}
    End Class
End Namespace
