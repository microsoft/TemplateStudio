# Advanced App navigation

This documentation describes the steps to modify the NavigationService to allow navigation in different frames and  navigation levels to support advanced navigation scenarios in a Navigation Pane project with frame work MVVM Basic or CodeBehind.

Scenarios covered in this document:

- Show a Startup page on app launching and navigate to a navigation pane shell page from there.
- Navigate from the navigation pane to a page in full screen mode.
- Navigate giving the possibility to go back using the back button or navigate without the posibility to go back.
- Reset navigation.

## Modifications in project (for all scenarios)

**Files to replace:**

- NavigationService.cs

**Files to add:**

- NavigationConfig.cs
- NavigationArgs.cs
- NavigationBackStackEntry.cs

**Files to modify:**

- ActivationService.cs
- DefaultActivationHandler.cs
- App.xaml.cs

### 1. Replace NavigationService.cs

The NavigationService allows you to handle different navigation levels and track the the whole navigation stack in different frames to go back across the navigation tree.

You need to add this code replacing the current NavigationService class.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace YOUR_APP_NAME.Services
{
    public static class NavigationService
    {
        // This NavigationService can handle navigation in an application that uses various frames.
        // The NavigationService manages a global backstack with entries from all frames.
        // Internally the NavigationService uses these frame keys to idenitify the frame in navigation and manage the navigation back stack.
        private const string FrameKeyMain = "Main";
        private const string FrameKeySecondary = "Secondary";
        // You can add more frame key constants if your app manage more frames.

        public static event EventHandler<NavigationArgs> Navigated;
        public static event NavigationFailedEventHandler NavigationFailed;

        private static readonly Dictionary<string, Frame> _frames = new Dictionary<string, Frame>();
        private static readonly List<NavigationBackStackEntry> _backStack = new List<NavigationBackStackEntry>();

        public static bool InitializeMainFrame(Frame mainFrame)
        {
            if (InitializeFrame(FrameKeyMain, mainFrame))
            {
                // Place the main frame in the current Window
                Window.Current.Content = mainFrame;
                return true;
            }
            return false;
        }

        public static bool InitializeSecondaryFrame(Frame secondary)
        {
            return InitializeFrame(FrameKeySecondary, secondary);
        }

        private static bool InitializeFrame(string frameKey, Frame frame)
        {
            if (!_frames.ContainsKey(frameKey))
            {
                // When a new frame is initialized the frame key is associated with the frame's Tag property to recover it in navigation events.
                frame.Tag = frameKey;
                frame.Navigated += OnFrameNavigated;
                frame.NavigationFailed += OnFrameNavigationFailed;
                _frames.Add(frameKey, frame);
                return true;
            }
            return false;
        }

        public static bool IsMainFrameInitialized()
        {
            return IsInitialized(FrameKeyMain);
        }

        public static bool IsSecondaryFrameInitialized()
        {
            return IsInitialized(FrameKeySecondary);
        }

        private static bool IsInitialized(string frameKey)
        {
            var frame = _frames.GetValueOrDefault(frameKey);
            return frame?.Content != null;
        }

        public static bool CanGoBack => _backStack.Any();

        public static void GoBack()
        {
            if (CanGoBack)
            {
                var stackEntry = _backStack.First();
                var frame = GetFrame(stackEntry.FrameKey);
                frame.GoBack();
            }
        }

        public static bool NavigateInMainFrame<T>(NavigationConfig config = null)
            where T : Page
            => Navigate<T>(FrameKeyMain, config);

        public static bool NavigateInSecondaryFrame<T>(NavigationConfig config = null)
            where T : Page
            => Navigate<T>(FrameKeySecondary, config);

        private static bool Navigate<T>(string frameKey, NavigationConfig config = null)
            where T : Page
            => Navigate(typeof(T), frameKey, config);

        public static bool NavigateInMainFrame(Type pageType, NavigationConfig config = null)
            => Navigate(pageType, FrameKeyMain, config);

        public static bool NavigateInSecondaryFrame(Type pageType, NavigationConfig config = null)
            => Navigate(pageType, FrameKeySecondary, config);

        private static bool Navigate(Type pageType, string frameKey, NavigationConfig config = null)
        {
            config = config ?? NavigationConfig.Default;

            var frame = GetFrame(frameKey);
            if (frame.Content == null || frame.Content.GetType() != pageType)
            {
                var result = frame.Navigate(pageType, config.Parameter, config.InfoOverride);
                if (result)
                {
                    if (frame.CanGoBack) // False on first navigation
                    {
                        if (config.DisableBackNavigation)
                        {
                            // BackNavigation is disabled but the navigation is registered on the frame backstack. This navigation entry must be removed.
                            frame.BackStack.RemoveAt(0);
                        }
                        else
                        {
                            // Track a new NavigationBackStackEntry in the NavigationService backstack
                            _backStack.Insert(0, new NavigationBackStackEntry(frameKey, pageType, config));
                        }
                    }

                    // Raise the Navigated Event for new navigations
                    Navigated?.Invoke(frame, new NavigationArgs(frameKey, pageType, config, frame.Content));
                }
            }
            return false;
        }

        public static void ResetNavigation()
        {
            // Unregister all frame events and clear frames and global backstack
            foreach (var frame in _frames.Values)
            {
                frame.Navigated -= OnFrameNavigated;
                frame.NavigationFailed -= OnFrameNavigationFailed;
            }
            _frames.Clear();
            _backStack.Clear();
            var newFrame = new Frame();
            InitializeMainFrame(newFrame);
        }

        public static bool IsPageInMainFrame<T>()
            where T : Page
            => IsPageInFrame<T>(FrameKeyMain);

        public static bool IsPageInSecondaryFrame<T>()
            where T : Page
        => IsPageInFrame<T>(FrameKeySecondary);

        private static bool IsPageInFrame<T>(string frameKey)
            where T : Page
        {
            var frame = GetFrame(frameKey);
            return frame.Content != null && frame.Content is T;
        }

        public static bool HasContentMainFrame()
            => HasContent(FrameKeyMain);

        public static bool HasContentSecondaryFrame()
            => HasContent(FrameKeySecondary);

        private static bool HasContent(string frameKey)
        {
            var frame = GetFrame(frameKey);
            return frame.Content != null;
        }

        private static Frame GetFrame(string frameKey)
        {
            // returns the frame associated with the frame key
            var frame = _frames.GetValueOrDefault(frameKey);
            if (frame == null)
            {
                // throw error if the frame is not initialzed
                var methodName = frameKey == FrameKeyMain ? nameof(InitializeMainFrame) : nameof(InitializeFrame);
                throw new Exception($"Frame is not initialized, please call {methodName} before navigating.");
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
                    // remove from global back stack
                    _backStack.RemoveAt(0);
                }
                if (e.NavigationMode != NavigationMode.New)
                {
                    // Raise Navigated event when NavigationMode is back or refresh.
                    Navigated?.Invoke(frame, new NavigationArgs(frameKey, e));
                }
            }
        }

        private static void OnFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }
    }
}
```

### 2. NavigationConfig.cs

NavigationConfig represents the navigation configuration and allows you to specify navigation parameters and if you want to register the navigation on the back stack.

You need to add this class.

```csharp
using Windows.UI.Xaml.Media.Animation;

namespace YOUR_APP_NAME.Services
{
    public class NavigationConfig
    {
        public readonly bool DisableBackNavigation;

        public readonly object Parameter;

        public readonly NavigationTransitionInfo InfoOverride;

        public NavigationConfig(bool disableBackNavigation = false, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            DisableBackNavigation = disableBackNavigation;
            Parameter = parameter;
            InfoOverride = infoOverride;
        }

        public static NavigationConfig Default => new NavigationConfig();
    }
}
```

### 3. Add NavigationArgs.cs

NavigationArgs contains navigation arguments and the framekey the navigation took place on.

You need to add this class.

```csharp
using System;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace YOUR_APP_NAME.Services
{
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
}
```

### 4. Add NavigationBackStackEntry.cs

NavigationBackStackEntry represents an entry on the navigation backstack.
You need to add this class.

```csharp
using System;
using Windows.UI.Xaml.Media.Animation;

namespace YOUR_APP_NAME.Services
{
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
}
```

### 5. Changes in DefaultActivationHandler.cs

- Change the DefaultActivationHandler to:

```csharp
internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
{
    private readonly Type _navElement;
    private readonly Type _shell;

    public DefaultActivationHandler(Type navElement, Type shell = null)
    {
        _navElement = navElement;
        _shell = shell;
    }

    protected override async Task HandleInternalAsync(IActivatedEventArgs args)
    {
        // When the navigation stack isn't restored, navigate to the first page and configure
        // the new page by passing required information in the navigation parameter
        object arguments = null;
        if (args is LaunchActivatedEventArgs launchArgs)
        {
            arguments = launchArgs.Arguments;
        }
        if (_shell != null)
        {
            NavigationService.NavigateInMainFrame(_shell);
            NavigationService.NavigateInSecondaryFrame(_navElement, new NavigationConfig(disableBackNavigation: false, parameter: arguments));
        }
        else
        {
            NavigationService.NavigateInMainFrame(_navElement, new NavigationConfig(disableBackNavigation: false, parameter: arguments));
        }


        await Task.CompletedTask;
    }

    protected override bool CanHandleInternal(IActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the app activation
        return !NavigationService.HasContentMainFrame();
    }
}
```

- Change the method `ActivateAsync()` to

```csharp
public async Task ActivateAsync(object activationArgs)
{
    if (IsInteractive(activationArgs))
    {
        // Initialize services that you need before app activation
        // take into account that the splash screen is shown while this code runs.
        await InitializeAsync();

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (Window.Current.Content == null)
        {
            var frame = new Frame();
            NavigationService.InitializeMainFrame(frame);
        }
    }

    // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
    // will navigate to the first page
    await HandleActivationAsync(activationArgs);
    _lastActivationArgs = activationArgs;

    if (IsInteractive(activationArgs))
    {
        // Ensure the current window is active
        Window.Current.Activate();

        // Tasks after activation
        await StartupAsync();
    }
}
```

- Add `_shell` parameter in `DefaultActivationHandler` constructor in `HandleActivationAsync`.

### 7. Change App.xaml.cs

#### 7.1 For startup on Startup page:

- Change method `CreateActivationService()` to

```csharp
private ActivationService CreateActivationService()
{
    return new ActivationService(this, typeof(Views.StartupPage));
}
```

#### 7.2 For startup on NavigationPane page:

- Change method `CreateActivationService()` to

```csharp
private ActivationService CreateActivationService()
{
    return new ActivationService(this, typeof(Views.MainPage), typeof(Views.ShellPage));
}

// Also remove this unreferenced method
private UIElement CreateShell()
{
    return new Views.ShellPage();
}
```

### 8. Changes in ShellPage.xaml, ShellPage.xaml.cs/ShellViewModel

- Add `NavigationCacheMode="Required"` to ShellPage.xaml
- Initialize secondary frame on ShellPage adding the following code to Initialize method on ShellViewModel.cs or ShellPage.xaml.cs

```csharp
NavigationService.InitializeSecondaryFrame(frame);

// Instead of
// NavigationService.Frame = frame;
```

- Replace `e` parameter from `NavigationEventArgs` to `NavigationArgs` in `OnNavigated` method.
- Add `NavigationService.IsPageInMainFrame` validation in method.

```csharp
private void OnNavigated(object sender, NavigationArgs e)
{
    // Handle navigation only when ShellPage in MainFrame
    if (NavigationService.IsPageInMainFrame<ShellPage>())
    {
        IsBackEnabled = NavigationService.CanGoBack;
        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
            return;
        }

        Selected = _navigationView.MenuItems
                        .OfType<WinUI.NavigationViewItem>()
                        .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
    }
}
```

- Change OnItemInvoked to navigate among pages with the new NavigationService.
Specify disableBackNavigation = true to specify that there will be no possibility to navigate back (use NavigateInSecondaryFrame intead of Navigate).

```csharp
private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
{
    if (args.IsSettingsInvoked)
    {
        NavigationService.NavigateInSecondaryFrame<SettingsPage>();
        return;
    }

    var item = _navigationView.MenuItems
                    .OfType<WinUI.NavigationViewItem>()
                    .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
    var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
    NavigationService.NavigateInSecondaryFrame(pageType);
}
```

- Update the `OnKeyboardAcceleratorInvoked` event handler.

```csharp
private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
{
    if (NavigationService.CanGoBack)
    {
        NavigationService.GoBack();
        args.Handled = true;
    }
}
```

### 9. Changes in NavigationViewHeaderBehavior.cs

- Update the `e` parameter from `NavigationEventArgs` to `NavigationArgs` in `OnNavigated` method.

## Navigate from Startup Page to NavigationPane page

To navigate to the shell page in fullscreen mode use NavigationService.NavigateInMainFrame and NavigationService.NavigateInSecondaryFrame to show the mainpage in the naviagation pane

```csharp
NavigationService.NavigateInMainFrame<ShellPage>(new NavigationConfig(disableBackNavigation: true));
NavigationService.NavigateInSecondaryFrame<MainPage>();
```

## Expand a Page to fullscreen/Navigate to a page in fullscreen

To navigate to a page in fullscreen mode use NavigationService.NavigateInMainFrame.

```csharp
NavigationService.NavigateInMainFrame<MapPage>();
```

To determine if page is already in fullscreen use the following code:

```csharp
return !NavigationService.IsPageInMainFrame<MapPage>();
```

## Reset navigation

To reset all frames and backstack (for example before navigating to the startup page) use the following code:

```csharp
NavigationService.ResetNavigation();
NavigationService.IsPageInMainFrame<StartUpPage>();
```
