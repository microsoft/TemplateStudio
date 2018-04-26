Imports Param_RootNamespace.Activation
Imports Param_RootNamespace.Helpers
Imports Windows.Storage
Imports Windows.UI.Notifications
Imports Windows.UI.StartScreen

Namespace Services
    Friend Partial Class LiveTileFeatureService
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)

        Private Const QueueEnabledKey As String = "LiveTileNotificationQueueEnabled"

        Public Async Function EnableQueueAsync() As Task
            Dim queueEnabled = Await ApplicationData.Current.LocalSettings.ReadAsync(Of Boolean)(QueueEnabledKey)
            If Not queueEnabled Then
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(True)
                Await ApplicationData.Current.LocalSettings.SaveAsync(QueueEnabledKey, True)
            End If
        End Function

        Public Sub UpdateTile(notification As TileNotification)
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification)
        End Sub

        Public Async Function PinSecondaryTileAsync(tile As SecondaryTile, Optional allowDuplicity As Boolean = False) As Task(Of Boolean)
            If Not Await IsAlreadyPinnedAsync(tile) OrElse allowDuplicity Then
                Return Await tile.RequestCreateAsync()
            End If

            Return False
        End Function

        Private Async Function IsAlreadyPinnedAsync(tile As SecondaryTile) As Task(Of Boolean)
            Dim secondaryTiles = Await SecondaryTile.FindAllAsync()
            Return secondaryTiles.Any(Function(t) t.Arguments = tile.Arguments)
        End Function

        Protected Overrides Async Function HandleInternalAsync(args As LaunchActivatedEventArgs) As Task
            ' If app is launched from a SecondaryTile, tile arguments property is contained in args.Arguments
            ' Dim secondaryTileArguments = args.Arguments

            ' If app is launched from a LiveTile notification update, TileContent arguments property is contained in args.TileActivatedInfo.RecentlyShownNotifications
            ' Dim tileUpdatesArguments = args.TileActivatedInfo.RecentlyShownNotifications
            Await Task.CompletedTask
        End Function

        Protected Overrides Function CanHandleInternal(args As LaunchActivatedEventArgs) As Boolean
            Return LaunchFromSecondaryTile(args) OrElse LaunchFromLiveTileUpdate(args)
        End Function

        Private Function LaunchFromSecondaryTile(args As LaunchActivatedEventArgs) As Boolean
            ' If app is launched from a SecondaryTile, tile arguments property is contained in args.Arguments
            ' TODO WTS: Implement your own logic to determine if you can handle the SecondaryTile activation
            Return False
        End Function

        Private Function LaunchFromLiveTileUpdate(args As LaunchActivatedEventArgs) As Boolean
            ' If app is launched from a LiveTile notification update, TileContent arguments property is contained in args.TileActivatedInfo.RecentlyShownNotifications
            ' TODO WTS: Implement your own logic to determine if you can handle the LiveTile notification update activation
            Return False
        End Function
    End Class
End Namespace
