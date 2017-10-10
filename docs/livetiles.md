# Live Tiles
LiveTile feature allows developers to show information in your to the final user through live tile icon updates.

LiveTile feature adds two classes to your project:

**LiveTileService.cs** is used to send LiveTile updates, pin SecondaryTiles to app sections and handle LiveTile activation.

**LiveTileService.Samples.cs** build samples of LiveTile and Secondary tiles to show how to use LiveTileService.

## Add Live Tile
To send a LiveTile update with `LiveTileService` it's necessary to build a new `TileNotification` object and then pass it to `UpdateTile` method.

```csharp
public void SampleUpdate()
{
    // Construct the tile content
    var content = new TileContent()
    {
        Visual = new TileVisual()
        {
            Arguments = "Jennifer Parker",
            TileMedium = new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    // Add here the configuration of medium tile content
                }
            },

            TileWide = new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    // Add here the configuration of wide tile content
                }
            }
        }
    };

    // Then create the tile notification
    var notification = new TileNotification(content.GetXml());
    UpdateTile(notification);
}
```

`UpdateTile` method call directly to Windows LiveTile API
```csharp
public void UpdateTile(TileNotification notification)
{
    TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
}
```

## Add Secondary Tile
To add a SecondaryTile with `LiveTileService` it's necessary to build a new `SecondaryTile` object and then pass it to `PinSecondaryTileAsync` method.
```csharp
public async Task SamplePinSecondaryAsync(string pageName)
{
    var tile = new SecondaryTile(DateTime.Now.Ticks.ToString());
    tile.Arguments = pageName;
    tile.DisplayName = pageName;
    tile.VisualElements.Square44x44Logo = new Uri("ms-appx:///Assets/Square44x44Logo.scale-200.png");
    tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png");
    tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.scale-200.png");
    tile.VisualElements.ShowNameOnSquare150x150Logo = true;
    tile.VisualElements.ShowNameOnWide310x150Logo = true;

    await PinSecondaryTileAsync(tile);
}
```
`PinSecondaryTileAsync` will check that tile parameter can be pinned before add the secondary tile.
```csharp
public async Task<bool> PinSecondaryTileAsync(SecondaryTile tile, bool allowDuplicity = false)
{
    if (!await IsAlreadyPinnedAsync(tile) || allowDuplicity)
    {
        return await tile.RequestCreateAsync();
    }

    return false;
}
```

## Handle app activation
`LiveTileService` is created as ActivationHandler of `LaunchActivatedEventArgs` and added in `ActivationService` class to `GetActivationHandlers` method.
```csharp
private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<LiveTileService>.Instance;
}
```

`CanHandleInternal` method must decide using args information if those launch arguments cames from a `LiveTile` update or a `SecondaryTile`.
```csharp
protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
{
    return LaunchFromSecondaryTile(args) || LaunchFromLiveTileUpdate(args);
}

private bool LaunchFromSecondaryTile(LaunchActivatedEventArgs args)
{
    return args.Arguments == SecondarySectionPageID;
}

private bool LaunchFromLiveTileUpdate(LaunchActivatedEventArgs args)
{
    if (args.TileActivatedInfo != null)
    {
        var tileUpdatesArguments = args.TileActivatedInfo.RecentlyShownNotifications;
        if (tileUpdatesArguments.Count > 0)
        {
            var allArguments = tileUpdatesArguments.Select(i => i.Arguments).ToArray();
            return allArguments.Any(a => a == "Jennifer Parker");
        }
    }            
    return false;
}
```

If app activation could be handled by LiveTileService `HandleInternalAsync` method decide the target page to navigate.

```csharp
protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
{
    if (LaunchFromSecondaryTile(args))
    {
        NavigationService.Navigate<SecondarySectionPage>();
    }
    else if (LaunchFromLiveTileUpdate(args))
    {
        NavigationService.Navigate<LiveTileUpdatePage>();
    }            
    await Task.CompletedTask;
}
```
With this activation configuration with can check the following scenarios:

**If the application is closed**
- Open from Windows applications menu: The app will be started at MainPage.
- Open from LiveTile icon that is showing a LiveTile update: The app will be started at LaunchFromLiveTile page.
- Open from pinned SecondaryTile: The app will be started at SecondaryTile page.

**If the application is already opened**
Open the already opened application from a LiveTile update will produce a navigation to LaunchFromLiveTile or a navigation to SecondaryTile if it was opened from SecondaryTile.

The [LiveTileActivationSample app](../samples/livetile/LiveTileActivationSample/) shows this implementation in a simple app created using Windows Template Studio.

