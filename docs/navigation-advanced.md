# Advanced App navigation
This documentation describes the steps to modify the NavigationService to contain different frames and different navigation levels. You can check out the sample code [here](https://github.com/Microsoft/WindowsTemplateStudio/tree/dev/poc/AdvancedNavigationPaneProject).
These are the scenarios shown in the App:

**App launching**
 - The App starts in a Startup page that only contains a start button.
 - By clicking on Start button the App Navigates to a NavigationPane Shell without the possibility to come back.

**NavigationPane Shell**
 - The navigation pane contains the main, map and settings pages and you can navigate on that pages on the Secondary frame without the possibility to come back using the back button.
 - A ShellPage that adds a second frame to the navigation model. You can navigate between Main, Map and Settings page without the possibility to come back using the back button.
 - Map page adds a button to see on full screen using the main frame navigation.
 - WebView button opens a WebView page in the main frame.
 - LogOut restores all the navigation system and returns to the StartUp Page.

 With this NavigationService you can handle different navigation levels and track all the navigation stack in different Frames to allow you to come back across all the navigation tree.

## NavigationService
You need to add this code replacing the current NavigationService class.
```csharp
public static class NavigationService
{
    public const string FrameKeyMain = "Main";
    public const string FrameKeySecondary = "Secondary";
    public const string FrameKeyThird = "Third";

    public static event EventHandler<NavigationArgs> Navigated;
    public static event NavigationFailedEventHandler NavigationFailed;

    private static string _currentFrame;
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
            _currentFrame = FrameKeyMain;
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
            _currentFrame = frameKey;
            return true;
        }
        _currentFrame = frameKey;
        return false;
    }

    /// <summary>
    /// Gets a value that indicates whether there is a frame initialized identified with a key.
    /// </summary>
    /// <param name="frameKey">Key to identify the frame in the NavigationService.</param>
    public static bool IsInitialized(string frameKey = null)
    {
        frameKey = frameKey ?? _currentFrame;
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
    /// Navigate in the current frame using the default NavigationConfig.
    /// </summary>
    /// <typeparam name="T">Source Page Type for Frame navigation.</typeparam>
    public static bool Navigate<T>()
        where T : Page
        => Navigate(typeof(T));

    /// <summary>
    /// Navigate in the current frame using the default NavigationConfig.
    /// </summary>
    /// <param name="pageType">Source Page Type for Frame navigation.</param>
    public static bool Navigate(Type pageType)
        => Navigate(pageType, null, null);

    /// <summary>
    /// Navigate in a specific frame using the default NavigationConfig.
    /// </summary>
    /// <typeparam name="T">Source Page Type for Frame navigation.</typeparam>
    /// <param name="frameKey">Key that identifies the Frame to navigate.</param>
    public static bool Navigate<T>(string frameKey)
        where T : Page
        => Navigate(typeof(T), frameKey);

    /// <summary>
    /// Navigate in a specific frame using the default NavigationConfig.
    /// </summary>
    /// <param name="pageType">Source Page Type for Frame navigation.</param>
    /// <param name="frameKey">Key that identifies the Frame to navigate.</param>
    public static bool Navigate(Type pageType, string frameKey)
        => Navigate(pageType, frameKey, null);

    /// <summary>
    /// Navigate in the current frame using a specific NavigationConfig.
    /// </summary>
    /// <typeparam name="T">Source Page Type for Frame navigation.</typeparam>
    /// <param name="config">Parameters configuration to customize the navigation.</param>
    public static bool Navigate<T>(NavigationConfig config)
        where T : Page
        => Navigate(typeof(T), config);

    /// <summary>
    /// Navigate in the current frame using a specific NavigationConfig.
    /// </summary>
    /// <param name="pageType">Source Page Type for Frame navigation.</param>
    /// <param name="config">Parameters configuration to customize the navigation.</param>
    public static bool Navigate(Type pageType, NavigationConfig config)
        => Navigate(pageType, null, config);

    /// <summary>
    /// Navigate in a specific frame using a specific NavigationConfig.
    /// </summary>
    /// <typeparam name="T">Source Page Type for Frame navigation.</typeparam>
    /// <param name="frameKey">Key that identifies the Frame to navigate.</param>
    /// <param name="config">Parameters configuration to customize the navigation.</param>
    public static bool Navigate<T>(string frameKey, NavigationConfig config)
        where T : Page
        => Navigate(typeof(T), frameKey, config);

    /// <summary>
    /// Navigate in a specific frame using a specific NavigationConfig.
    /// </summary>
    /// <param name="pageType">Source Page Type for Frame navigation.</param>
    /// <param name="frameKey">Key that identifies the Frame to navigate.</param>
    /// <param name="config">Parameters configuration to customize the navigation.</param>
    public static bool Navigate(Type pageType, string frameKey, NavigationConfig config)
    {
        frameKey = frameKey ?? _currentFrame;
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

    public NavigationConfig()
    {
        RegisterOnBackStack = true;
    }

    public NavigationConfig(bool registerOnBackStack)
    {
        RegisterOnBackStack = registerOnBackStack;
    }

    public NavigationConfig(object parameter)
    {
        Parameter = parameter;
    }

    public NavigationConfig(bool registerOnBackStack, object parameter)
    {
        RegisterOnBackStack = registerOnBackStack;
        Parameter = parameter;
    }

    public NavigationConfig(bool registerOnBackStack, object parameter, NavigationTransitionInfo infoOverride)
    {
        RegisterOnBackStack = registerOnBackStack;
        Parameter = parameter;
        InfoOverride = infoOverride;            
    }

    public static NavigationConfig Default => new NavigationConfig();
}
```

## Using the advanced NavigationService
### From StartUpPage to ShellPage
 - Navigate to ShellPage and this will add a new SecondaryFrame that will be set as default frame.
 - Then you can Navigate to MainPage to set the first page un the SecondFrame.

```csharp
NavigationService.Navigate<ShellPage>(new NavigationConfig(false));
NavigationService.Navigate<MainPage>();
```

### ShellPage menu item pages.

Navigate to NavigationViewItem NavigatedTo PageType using the NavigationConfig constructor to specify that this navigation will not be registered on navigation back-stack.
```csharp
private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
{
    if (args.IsSettingsInvoked)
    {
        NavigationService.Navigate<SettingsPage>(new NavigationConfig(false));
        return;
    }

    var item = _navigationView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
    var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
    NavigationService.Navigate(pageType, null, new NavigationConfig(false));
}
```

### Expanding MapPage

ShellPage navigates to MapPage on the SecondaryFrame, we are going to Navigate to MapPage in the MainFrame to view the page in full-screen mode.

```csharp
NavigationService.Navigate<MapPage>(NavigationService.FrameKeyMain);
```

### WebSite full-screen
In case of WebSite option, the navigation will also be realized in the main frame.

```csharp
NavigationService.Navigate<WebSitePage>(NavigationService.FrameKeyMain);
```

### Navigate to SecondShell

```csharp
NavigationService.Navigate<SecondShellPage>(NavigationService.FrameKeyMain);
NavigationService.Navigate<SecondMainPage>();
```