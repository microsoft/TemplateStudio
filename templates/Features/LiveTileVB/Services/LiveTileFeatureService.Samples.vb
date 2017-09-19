Imports Microsoft.Toolkit.Uwp.Notifications
Imports System.Threading.Tasks
Imports Windows.UI.Notifications
Imports Windows.UI.StartScreen

Namespace Services
    Partial Friend Class LiveTileFeatureService
        Public Sub SampleUpdate()
            ' See more information about Live Tiles Notifications
            ' Documentation: https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-sending-a-local-tile-notification

            ' These would be initialized with actual data
            Dim from As String = "Jennifer Parker"
            Dim subject As String = "Photos from our trip"
            Dim body As String = "Check out these awesome photos I took while in New Zealand!"

            ' Construct the tile content

            Dim content = New TileContent() With {
                .Visual = New TileVisual() With {
                    .Arguments = "Jennifer Parker",
                    .TileMedium = New TileBinding With {
                        .content = New TileBindingContentAdaptive With {
                            .Children = {New AdaptiveText With {
                                .Text = from
                            }, New AdaptiveText With {
                                .Text = subject,
                                .HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }, New AdaptiveText With {
                                .Text = body,
                                .HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }}
                        }
                    },
                    .TileWide = New TileBinding With {
                        .content = New TileBindingContentAdaptive() With {
                            .Children = {New AdaptiveText() With {
                                .Text = from,
                                .HintStyle = AdaptiveTextStyle.Subtitle
                            }, New AdaptiveText With {
                                .Text = subject,
                                .HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }, New AdaptiveText With {
                                .Text = body,
                                .HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }}
                        }
                    }
                }
            }

            ' Then create the tile notification
            Dim notification = New TileNotification(content.GetXml())
            UpdateTile(notification)
        End Sub

        Public Async Function SamplePinSecondaryAsync(pageName As String) As Task
            ' TODO WTS: Call this method to Pin a Secondary Tile from a page.
            ' You also must implement the navigation to this specific page in the OnLaunched event handler on App.xaml.cs
            Dim tile = New SecondaryTile(DateTime.Now.Ticks.ToString())
            tile.Arguments = pageName
            tile.DisplayName = pageName
            tile.VisualElements.Square44x44Logo = New Uri("ms-appx:///Assets/Square44x44Logo.scale-200.png")
            tile.VisualElements.Square150x150Logo = New Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png")
            tile.VisualElements.Wide310x150Logo = New Uri("ms-appx:///Assets/Wide310x150Logo.scale-200.png")
            tile.VisualElements.ShowNameOnSquare150x150Logo = True
            tile.VisualElements.ShowNameOnWide310x150Logo = True
            Await PinSecondaryTileAsync(tile)
        End Function
    End Class
End Namespace
