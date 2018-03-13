'{[{
Imports Microsoft.Toolkit.Uwp.UI.Extensions
'}]}
Namespace Views

    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
            '{[{
            AddHandler Loaded, AddressOf wts.ItemNamePage_Loaded
            '}]}
        End Sub

        '{[{

        Private Sub wts.ItemNamePage_Loaded(sender As Object, e As RoutedEventArgs)
            Dim element = TryCast(Me, FrameworkElement)
            Dim pivotPage = element.FindAscendant(Of Pivot)()
            If pivotPage IsNot Nothing Then
                AddHandler pivotPage.SelectionChanged, AddressOf PivotPage_SelectionChanged
            End If

            AddHandler mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged, AddressOf PlaybackSession_PlaybackStateChanged
            mpe.MediaPlayer.Play()
        End Sub

        Private Sub PivotPage_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            Dim navigatedTo As Boolean = e.AddedItems.Cast(Of PivotItem)().Any(Function(p) p.FindDescendant(Of wts.ItemNamePage)() IsNot Nothing)
            Dim navigatedFrom As Boolean = e.RemovedItems.Cast(Of PivotItem)().Any(Function(p) p.FindDescendant(Of wts.ItemNamePage)() IsNot Nothing)
            If navigatedTo Then
                AddHandler mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged, AddressOf PlaybackSession_PlaybackStateChanged
            End If

            If navigatedFrom Then
                mpe.MediaPlayer.Pause()
                RemoveHandler mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged, AddressOf PlaybackSession_PlaybackStateChanged
            End If
        End Sub
        '}]}
    End Class
End Namespace
