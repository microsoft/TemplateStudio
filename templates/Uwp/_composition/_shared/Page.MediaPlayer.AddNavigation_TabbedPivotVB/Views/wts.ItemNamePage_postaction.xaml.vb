'{[{
Imports Param_ItemNamespace.Helpers
'}]}
Namespace Views

    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
        End Sub

        '{[{
        Public Async Function OnPivotSelectedAsync() As Task Implements IPivotPage.OnPivotSelectedAsync
            mpe.MediaPlayer.Play()
            AddHandler mpe.MediaPlayer.PlaybackSession.PlaybackRateChanged, AddressOf PlaybackSession_PlaybackStateChanged
            Await Task.CompletedTask
        End Function

        Public Async Function OnPivotUnselectedAsync() As Task Implements IPivotPage.OnPivotUnselectedAsync
            mpe.MediaPlayer.Pause()
            RemoveHandler mpe.MediaPlayer.PlaybackSession.PlaybackRateChanged, AddressOf PlaybackSession_PlaybackStateChanged
            Await Task.CompletedTask
        End Function
        '}]}
    End Class
End Namespace
