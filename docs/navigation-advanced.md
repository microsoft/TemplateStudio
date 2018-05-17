# Advanced App navigation
This documentation describes the steps to modify the NavigationService to allow navigation in different frames and  navigation levels to support advanced navigation scenarios, like the following:

**App launching**
 - Show a Startup page on app launching and navigate to a navigation pane shell page from there. 
 - Give the possibility to go back to the startup page or not.
 
**NavigationPane Scenarios**
 - Navigate among the navigation pane pages with or without the possibility to navigate back using the back button.
 - Navigate to a page in full screen mode.
 - Navigate to a logout page that goes back to the startup page and resets the navigation.
 
 The NavigationService allows you to handle different navigation levels and track all the navigation stack in different Frames to to go back across all the navigation tree.

## NavigationService
You need to add this code replacing the current NavigationService class.
```csharp
public static class NavigationService
{
    public const string FrameKeyMain = "Main";
    public const string FrameKeySecondary = "Secondary";

    public static event EventHandler<NavigationArgs> Navigated;
    public static event NavigationFailedEventHandler NavigationFailed;

    private static readonly Dictionary<string, Frame> _frames = new Dictionary<string, Frame>();
    private static readonly List<NavigationBackStackEntry> _backStack = new List<NavigationBackStackEntry>();

    /// <summary>
    /// Register the main frame in the NavigationService.
    /// </summary>
    /// <param name="mainFrame">New frame to register in the NavigationService</param>
    public static bool InitializeMainFrame(Frame mainFrame)
    {
        if (InitializeFrame(FrameKeyMain, mainFrame))
        {
            Window.Current.Content = mainFrame;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Register a frame in the NavigationService using a specific frame key.
    /// </summary>
    /// <param name="frameKey">Key that will identify the frame in the NavigationService.</param>
    /// <param name="frame">New frame to register in the NavigationService.</param>
    public static bool InitializeFrame(string frameKey, Frame frame)
    {
        if (!_frames.ContainsKey(frameKey))
        {
            frame.Tag = frameKey;
            frame.Navigated += OnFrameNavigated;
            frame.NavigationFailed += OnFrameNavigationFailed;
            _frames.Add(frameKey, frame);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets a value that indicates whether there is a frame initialized identified with a key.
    /// </summary>
    /// <param name="frameKey">Key to identify the frame in the NavigationService.</param>
    public static bool IsInitialized(string frameKey)
    {
        var frame = _frames.GetValueOrDefault(frameKey);
        return frame?.Content != null;
    }

    /// <summary>
    /// Gets a value that indicates whether there is at least one entry in back navigation history.
    /// </summary>
    public static bool CanGoBack => _backStack.Any();

    /// <summary>
    /// Navigates to the most recent item in back navigation history.
    /// </summary>
    public static void GoBack()
    {
        if (CanGoBack)
        {
            var stackEntry = _backStack.First();
            var frame = GetFrame(stackEntry.FrameKey);
            frame.GoBack();
        }
    }

    /// <summary>
    /// Navigate in a specific frame using a specific NavigationConfig.
    /// </summary>
    /// <typeparam name="T">Source Page Type for Frame navigation.</typeparam>
    /// <param name="frameKey">Key that identifies the Frame to navigate.</param>
    /// <param name="config">Parameters configuration to customize the navigation.</param>
    public static bool Navigate<T>(string frameKey, NavigationConfig config = null)
        where T : Page
        => Navigate(typeof(T), frameKey, config);

    /// <summary>
    /// Navigate in a specific frame using a specific NavigationConfig.
    /// </summary>
    /// <param name="pageType">Source Page Type for Frame navigation.</param>
    /// <param name="frameKey">Key that identifies the Frame to navigate.</param>
    /// <param name="config">Parameters configuration to customize the navigation.</param>
    public static bool Navigate(Type pageType, string frameKey, NavigationConfig config = null)
    {
        config = config ?? NavigationConfig.Default;

        var frame = GetFrame(frameKey);
        if (frame.Content == null || frame.Content.GetType() != pageType)
        {
            var result = frame.Navigate(pageType, config.Parameter, config.InfoOverride);
            if (result)
            {
                if (frame.CanGoBack)
                {
                    if (config.RegisterOnBackStack)
                    {
                        _backStack.Insert(0, new NavigationBackStackEntry(frameKey, pageType, config));
                    }
                    else
                    {
                        frame.BackStack.RemoveAt(0);
                    }
                }
                Navigated?.Invoke(frame, new NavigationArgs(frameKey, pageType, config, frame.Content));
            }
        }
        return false;
    }

    public static void ResetNavigation()
    {
        foreach (var frame in _frames.Values)
        {
            frame.Navigated -= OnFrameNavigated;
            frame.NavigationFailed -= OnFrameNavigationFailed;
        }
        _frames.Clear();
        _backStack.Clear();
        var newFrame = new Frame();
        InitializeMainFrame(newFrame);
        ThemeSelectorService.SetRequestedTheme();
    }

    public static bool IsPageInFrame<T>(string frameKey)
        where T : Page
    {
        var frame = GetFrame(frameKey);
        return frame.Content != null && frame.Content is T;
    }

    private static Frame GetFrame(string frameKey)
    {
        var frame = _frames.GetValueOrDefault(frameKey);
        if (frame == null)
        {
            var methodName = frameKey == FrameKeyMain ? nameof(InitializeMainFrame) : nameof(InitializeFrame);
            throw new Exception($"Frame is not initialized, please call {methodName} before navigate.");
        }
        return frame;
    }

    private static void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var frameKey = frame.Tag as string;
            if (e.NavigationMode == NavigationMode.Back)
            {
                _backStack.RemoveAt(0);
            }
            if (e.NavigationMode != NavigationMode.New)
            {
                Navigated?.Invoke(frame, new NavigationArgs(frameKey, e));
            }
        }
    }

    private static void OnFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        NavigationFailed?.Invoke(sender, e);
    }
}
```

## NavigationArgs
You need to add this additional class.

```csharp
public class NavigationArgs : EventArgs
{
    public readonly string FrameKey;

    public readonly Uri Uri;

    public readonly object Content;

    public readonly NavigationMode NavigationMode;

    public readonly object Parameter;

    public readonly Type SourcePageType;

    public readonly NavigationTransitionInfo NavigationTransitionInfo;

    public NavigationArgs(string frameKey, NavigationEventArgs args)
    {
        FrameKey = frameKey;
        SourcePageType = args.SourcePageType;
        Parameter = args.Parameter;
        NavigationMode = args.NavigationMode;
        Content = args.Content;
        NavigationTransitionInfo = args.NavigationTransitionInfo;
        Uri = args.Uri;
    }

    public NavigationArgs(string frameKey, Type sourcePageType, NavigationConfig config, object content)
    {
        FrameKey = frameKey;
        SourcePageType = sourcePageType;
        Parameter = config.Parameter;
        NavigationMode = NavigationMode.New;
        Content = content;
        NavigationTransitionInfo = config.InfoOverride;
    }
}
```

## NavigationBackStackEntry
You need to add this additional class.
```csharp
public class NavigationBackStackEntry
{
    public readonly string FrameKey;        

    public readonly object Parameter;

    public readonly Type SourcePageType;

    public readonly NavigationTransitionInfo NavigationTransitionInfo;

    public NavigationBackStackEntry(string frameKey, Type sourcePageType, NavigationConfig config)
    {
        FrameKey = frameKey;
        SourcePageType = sourcePageType;
        Parameter = config.Parameter;
        NavigationTransitionInfo = config.InfoOverride;
    }
}
```

## NavigationConfig
You need to add this additional class.
```csharp
public class NavigationConfig
{
    public readonly bool RegisterOnBackStack;

    public readonly object Parameter;

    public readonly NavigationTransitionInfo InfoOverride;        

    public NavigationConfig(bool registerOnBackStack = true, object parameter = null, NavigationTransitionInfo infoOverride = null)
    {
        RegisterOnBackStack = registerOnBackStack;
        Parameter = parameter;
        InfoOverride = infoOverride;            
    }

    public static NavigationConfig Default => new NavigationConfig();
}
```
## Changes in project: 
- Initialize the main frame from ActivationService.ActivateAsync() replacing `Window.Current.Content = _shell?.Value ?? new Frame();` by 
 ```csharp
var frame = new Frame();
if (_shell?.Value != null)
{
    frame.Content = _shell.Value;
}
NavigationService.InitializeMainFrame(frame);
```

- Replace `NavigationService.Navigated += Frame_Navigated;` by `NavigationService.Navigated += OnNavigated;` and implement event handler:

 ```csharp
 private void OnNavigated(object sender, NavigationArgs e)
{
    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationService.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
}
```

-  Change HandleInternalAsync on DefaultLaunchActivationHandler 
to `            NavigationService.Navigate(_navElement, NavigationService.FrameKeyMain, new NavigationConfig(registerOnBackStack: true, parameter: args.Arguments));
`

- Change CanHandleInternal on DefaultLaunchActivationHandler  to 
`return !NavigationService.IsInitialized(NavigationService.FrameKeyMain);`

## Using the advanced NavigationService

### Navigate to startup page
- On App.xaml.cs in CreateActivationService replace the existing code with `return new ActivationService(this, typeof(Views.StartUpPage));` to navigate directly to the StartupPage

### Navigate from StartUpPage to ShellPage
#### ShellPage changes:
-  Add NavigationCacheMode="Required" to ShellPage.xaml.cs
 - Initialize secondary frame on ShellPage adding the following code to Initialize method on ShellViewModel or ShellPage.xaml.cs
 ```csharp
 NavigationService.InitializeFrame(NavigationService.FrameKeySecondary, shellFrame);
 ```
 - Subscribe to NavigationService.Navigated event, implement the eventHandler 
 ```csharp
 private void OnNavigated(object sender, NavigationArgs e)
{
    // Handle navigation only for navigations on secondary frame
    if (e.FrameKey == NavigationService.FrameKeySecondary)
    {
        Selected = navigationView.MenuItems
                    .OfType<NavigationViewItem>()
                    .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
    }
}
```
- Unregister the event handler when navigating away.

#### On the StartupPage:

 - Navigate to ShellPage (setting registerOnBackstack to false will cause the navigation not to be added to the backstack, so you won't be able to go back.
 - Navigate to MainPage to set the first page in the SecondFrame.

```csharp
NavigationService.Navigate<ShellPage>(NavigationService.FrameKeyMain, new NavigationConfig(registerOnBackStack: false));

NavigationService.Navigate<MainPage>();
```

### Navigate among NavigationPane item pages.

Navigate to NavigationViewItem using NavigationService.Navigate. 
Specify registerOnBackStack = false to specify that this navigation will not be registered on navigation back-stack.
```csharp
private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
{
    if (args.IsSettingsInvoked)
    {
        NavigationService.Navigate<SettingsPage>(NavigationService.FrameKeySecondary, new NavigationConfig(registerOnBackStack: false));
        return;
    }

    var item = _navigationView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
    var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
    NavigationService.Navigate(pageType, NavigationService.FrameKeySecondary, new NavigationConfig(registerOnBackStack: false));
}
```

### Expand a Page to fullscreen/Navigate to a page on fullscreen

To navigate to a page on fullscreen mode use NavigationService.Navigate specifying the MainFrame.

```csharp
NavigationService.Navigate<MapPage>(NavigationService.FrameKeyMain);
```

To determine if page is already in fullscreen use the following code:
```csharp
return !NavigationService.IsPageInFrame<MapPage>(NavigationService.FrameKeyMain);
```

### Navigate to Logoutpage and reset navigation

- On Logout command reset navigation and navigate to startup page calling

```csharp
NavigationService.ResetNavigation();
NavigationService.Navigate<StartUpPage>(NavigationService.FrameKeyMain);
```