Imports Microsoft.Toolkit.Uwp.Notifications
Imports Windows.UI.Notifications
Imports Windows.UI.StartScreen

Namespace Services
    Friend Partial Class LiveTileFeatureService

        ' More about Live Tiles Notifications at https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-sending-a-local-tile-notification
        Public Sub SampleUpdate()

            ' These would be initialized with actual data
            Dim from = "Jennifer Parker"
            Dim subject = "Photos from our trip"
            Dim body = "Check out these awesome photos I took while in New Zealand!"

            ' Construct the tile content
            Dim mediumTileContent = New TileBindingContentAdaptive()
            mediumTileContent.Children.Add(New AdaptiveText() With {.Text = from})
            mediumTileContent.Children.Add(New AdaptiveText() With {.Text = subject, .HintStyle = AdaptiveTextStyle.CaptionSubtle})
            mediumTileContent.Children.Add(New AdaptiveText() With {.Text = body, .HintStyle = AdaptiveTextStyle.CaptionSubtle})

            Dim wideTileContent = New TileBindingContentAdaptive()
            wideTileContent.Children.Add(New AdaptiveText() With {.Text = from, .HintStyle = AdaptiveTextStyle.Subtitle})
            wideTileContent.Children.Add(New AdaptiveText() With {.Text = subject, .HintStyle = AdaptiveTextStyle.CaptionSubtle})
            wideTileContent.Children.Add(New AdaptiveText() With {.Text = body, .HintStyle = AdaptiveTextStyle.CaptionSubtle})

            Dim content = New TileContent() With {
                .Visual = New TileVisual() With {
                    .Arguments = "Jennifer Parker",
                    .TileMedium = New TileBinding() With { .Content = mediumTileContent},
                    .TileWide = New TileBinding() With {.Content = wideTileContent}
                }
            }

            ' Then create the tile notification
            Dim notification = New TileNotification(content.GetXml())
            UpdateTile(notification)
        End Sub

        Public Async Function SamplePinSecondaryAsync(pageName As String) As Task
            ' TODO WTS: Call this method to Pin a Secondary Tile from a page.
            ' You also must implement the navigation to this specific page in the OnLaunched event handler on App.xaml.vb
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
