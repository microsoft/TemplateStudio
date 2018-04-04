# Navigation Pane

:heavy_exclamation_mark: There is also a version of [this document with code samples in VB.Net](./navigationpane.vb.md) :heavy_exclamation_mark: |
------------------------------------------------------------------------------------------------------------------------------------------------ |

The navigation pane project type includes a navigation menu displayed in a panel at the side of the screen and which can be expanded with the Hamburger icon.

This document covers:

* [Modifying the menu items](#menu)
* [Using the navigation pane with command bars](#commandbar)
* [Have the menu item invoke code rather than navigate](#invokecode)
* [Remove adaptive behaviors from HamburgerMenu](#adaptivebehaviors)

<a name="menu"></a>

## Modifying the menu items

The menu can be modified in the following ways.

* Change the icon for an item in the navigation panel menu
* Change the text for an item in the navigation panel menu

### Change the icon for an item

By default every item in the navigation pane is displayed with the symbol for a document.
When every item has the same icon it is hard to differentiate between them when the navigation panel is collapsed. In almost all cases you will want to change the icon used.

![](../resources/modifications/NavMenu_Different_Symbols.png)

Navigate to `ViewModel/ShellViewModel.cs` (or `Views/ShellPage.xaml.cs` if using Code Behind) and change the `PopulateNavItems` method.

The code below shows the symbols used to create the app shown in the image above.

```csharp
private void PopulateNavItems()
{
    _primaryItems.Clear();
    _secondaryItems.Clear();

    _primaryItems.Add(ShellNavigationItem.FromType<MainPage>("Shell_Main".GetLocalized(), Symbol.Home));
    _primaryItems.Add(ShellNavigationItem.FromType<MapPage>("Shell_Map".GetLocalized(), Symbol.Map));
    _primaryItems.Add(ShellNavigationItem.FromType<MasterDetailPage>("Shell_MasterDetail".GetLocalized(), Symbol.DockLeft));
    _primaryItems.Add(ShellNavigationItem.FromType<TabbedPage>("Shell_Tabbed".GetLocalized(), Symbol.Document)); // This is still the default
    _primaryItems.Add(ShellNavigationItem.FromType<WebViewPage>("Shell_WebView".GetLocalized(), Symbol.Globe));
    _secondaryItems.Add(ShellNavigationItem.FromType<SettingsPage>("Shell_Settings".GetLocalized(), Symbol.Setting));
}
```

The icons are created using the `Windows.UI.Xaml.Controls.Symbol` enumeration. You can view all the symbols available at <https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.symbol>

You can also set the menu item to use an `IconElement` directly. Like this:

```csharp
_navigationItems.Add(ShellNavigationItem.FromType<MainView>("Shell_Main".GetLocalized(), new FontIcon { Glyph = "\uED5A" }));
```

### Change the text for an item

The text for a shell navigation item comes from the localized string resources. For an item which defines the text with `"Shell_Main".GetLocalized()` the value "Shell_Main" corresponds with an entry in `Resources.resw`. Change the value in the resources file to alter what is displayed in the navigation menu.

<a name="commandbar"></a>

## Using the navigation pane and command bars

The following is intended as an aid to anyone wanting to add a `CommandBar` to one or all pages in an app using a Navigation Pane.

### Avoid page.TopAppBar and page.BottomAppBar

Each page has a property for `TopAppBar` and `BottomAppBar` intended to hold a `CommandBar`. Despite their names, they put the CommandBar at the top (or bottom) of the window, not the Page that declares it. Because the NavigationPane works by putting a page within the ShellPage this causes the CommandBar to overlap with the hamburger control and is not desirable. Instead use one of the techniques shown below.

### Adding a CommandBar to a single page

Because of the issue identified above, to add a `Commandbar` to the **bottom** of a single page, simply position it at the bottom of the grid intended for displaying content.

```xml
    <Grid 
        Grid.Row="1" 
        Background="{ThemeResource SystemControlPageBackgroundChromeLowBrush}">
        <!--The SystemControlPageBackgroundChromeLowBrush background represents where you should place your content. 
            Place your content here.-->

        <CommandBar VerticalAlignment="Bottom">
            <AppBarButton x:Uid="AdminButton" Icon="Admin" />
        </CommandBar>
    </Grid>
```

If adding the bar at the **top** of the page it can incorporate the page's title text in the content area. Like this:

```xml
    <Grid.RowDefinitions>
        <RowDefinition x:Name="TitleRow" Height="Auto" />
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>

    <CommandBar>
        <CommandBar.Content>
            <Grid>
                <TextBlock
                    x:Name="TitlePage"
                    x:Uid="Main_Title"
                    Text="Navigation Item 2"
                    Style="{StaticResource PageTitleStyle}" />
            </Grid>
        </CommandBar.Content>
        <AppBarButton Icon="Add" Label="Add" />
    </CommandBar>
```

### Adding a CommandBar to every page in the app

By adding a CommandBar to the `ShellPage` it is visible when navigating to any page.

To add `CommandBar` at the bottom of every page, modify the contents of `ShellPage.xaml` to add a `Grid` around the `HamburgerMenu` and also include a `CommandBar`.

```xml
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>
        <controls:HamburgerMenu>
            <!--  Contents of Hamburger menu omitted to brevity  -->
        </controls:HamburgerMenu>
        <CommandBar Grid.Row="1">
            <AppBarButton Icon="Camera" Label="Picture" />
        </CommandBar>
    </Grid>
```

The above approach can be used to put the bar above the `HamburgerMenu` by swapping the rows in the grid.
Alternatively, a bar can be added inside the Hamburger menu but above each pages content by adding it above the `Frame` inside the `HamburgerMenu`.

```xml
    <Grid Background="{ThemeResource SystemControlBackgroundAltHighBrush}">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <CommandBar>
            <AppBarButton Icon="Save" Label="Save" />
        </CommandBar>
        <Frame x:Name="shellFrame" Grid.Row="1" />
    </Grid>
```

**A Note about the above code examples.**

Events and commands are not shown in the above code but can easily be added like any other button click event or command. Note that if using the techniques for adding the bar to every page, the events or commands should be handled by the ShellPageViewModel (or in ShellPage.xaml.cs if using CodeBehind.)

In all the above code examples the `Label` values have been hard-coded. This is to make the samples simpler. To use localized text, set the `x:Uid` value for the `AppBarButton` and add a corresponding resource entry for "{name}.Label".

The examples also only show a single `AppBarButton` being added. This is to keep the code sample as simple as possible but you can add any appropriate content to the bar, as [documented here](https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/app-bars).

<a name="invokecode"></a>

## Have the menu item invoke code rather than navigate

Extending the app to add this functionality requires making two changes.

1. Change the ShellNavigationItem to be able to handle an `Action`.
1. Change the ShellPage or ShellViewModel to invoke the action.

In **ShellNavigationItem.cs** add the following

```csharp
    public Action Action { get; private set; }

    private ShellNavigationItem(string label, Symbol symbol, Action action)
        : this(label, null)
    {
        Symbol = symbol;
        Action = action;
    }

    public static ShellNavigationItem ForAction(string label, Symbol symbol, Action action)
    {
        return new ShellNavigationItem(label, symbol, action);
    }
```

### If using CodeBehind

In **ShellPage.xaml.cs** change the `Navigate` method to be like this.

```csharp
    private void Navigate(object item)
    {
        var navigationItem = item as ShellNavigationItem;
        if (navigationItem != null)
        {
            if (navigationItem.Action != null)
            {
                navigationItem.Action.Invoke();
            }
            else
            {
                NavigationService.Navigate(navigationItem.PageType);
            }
        }
    }
```

### If using MVVM Basic

In **ShellPageViewModel** change the `Navigate` method to be like this.

```csharp
    private void Navigate(object item)
    {
        var navigationItem = item as ShellNavigationItem;
        if (navigationItem != null)
        {
            if (navigationItem.Action != null)
            {
                navigationItem.Action.Invoke();
            }
            else
            {
                NavigationService.Navigate(navigationItem.PageType);
            }
        }
    }
```

### If using MVVM Light

In **ShellPageViewModel** change the `Navigate` method to be like this.

```csharp
    private void Navigate(object item)
    {
        var navigationItem = item as ShellNavigationItem;
        if (navigationItem != null)
        {
            if (navigationItem.Action != null)
            {
                navigationItem.Action.Invoke();
            }
            else
            {
                NavigationService.Navigate(navigationItem.ViewModelName);
            }
        }
    }
```

You can then add a menu item that uses the above.
e.g. In `PopulateNavItems()` add something like this:

```csharp
_secondaryItems.Add(
    ShellNavigationItem.ForAction(
        "Rate this app",
        Symbol.OutlineStar,
        async () => {
            await new Windows.UI.Popups.MessageDialog("5 stars please").ShowAsync();
            // .. code to launch the review process, etc.
        }));
```

<a name="adaptivebehaviors"></a>

## Remove adaptive behaviors from HamburgerMenu

WTS generates code that includes AdaptiveTriggers to provide responsive adaptation on window size changes. If you want to remove this behavior because your app doesn't need it or because you prefer not to use it, please follow this documentation.

The first thing you have to remove is the VisualStates and the DisplayMode property on ShellView.xaml.

### MVVM Basic - ShellView.xaml
**Remove the commented code**
```xml
<controls:HamburgerMenu
    x:Name="NavigationMenu"
    UseNavigationViewWhenPossible="True"
    SelectedItem="{x:Bind ViewModel.Selected, Mode=OneWay}"
    IsPaneOpen="{x:Bind ViewModel.IsPaneOpen, Mode=TwoWay}"
    ItemTemplate="{StaticResource NavigationMenuItemDataTemplate}"
    ItemsSource="{x:Bind ViewModel.PrimaryItems}"
    OptionsItemTemplate="{StaticResource NavigationMenuItemDataTemplate}"
    OptionsItemsSource="{x:Bind ViewModel.SecondaryItems}"
    PaneBackground="{ThemeResource SystemControlBackgroundAltHighBrush}"
    PaneForeground="{ThemeResource SystemControlForegroundBaseHighBrush}">
    <!--
    Remove this property from HamburgerMenu
    DisplayMode="{x:Bind ViewModel.DisplayMode, Mode=OneWay}"
    -->
    <i:Interaction.Behaviors>
        <ic:EventTriggerBehavior EventName="ItemInvoked">
            <ic:InvokeCommandAction Command="{x:Bind ViewModel.ItemSelectedCommand}" />
        </ic:EventTriggerBehavior>
    </i:Interaction.Behaviors>
    <Grid Background="{ThemeResource SystemControlBackgroundAltHighBrush}">
        <Frame x:Name="shellFrame"/>
    </Grid>
    <!--
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup x:Name="WindowStates">
            <VisualState x:Name="PanoramicState">
                <VisualState.StateTriggers>
                    <AdaptiveTrigger MinWindowWidth="1024"/>
                </VisualState.StateTriggers>
            </VisualState>
            <VisualState x:Name="WideState">
                <VisualState.StateTriggers>
                    <AdaptiveTrigger MinWindowWidth="640"/>
                </VisualState.StateTriggers>
            </VisualState>
            <VisualState x:Name="NarrowState">
                <VisualState.StateTriggers>
                    <AdaptiveTrigger MinWindowWidth="0"/>
                </VisualState.StateTriggers>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
    -->
</controls:HamburgerMenu>
```

You also have to remove the CurrentStateChanged management code from ShellViewModel.cs.

### MVVM Basic - ShellViewModel.cs

**Remove the commented code**

```csharp
// private const string PanoramicStateName = "PanoramicState";
// private const string WideStateName = "WideState";
// private const string NarrowStateName = "NarrowState";
// private const double WideStateMinWindowWidth = 640;
// private const double PanoramicStateMinWindowWidth = 1024;

// private SplitViewDisplayMode _displayMode = SplitViewDisplayMode.CompactInline;

// public SplitViewDisplayMode DisplayMode
// {
//         get { return _displayMode; }
//         set { Set(ref _displayMode, value); }
// }

// private ICommand _stateChangedCommand;

// public ICommand StateChangedCommand
// {
//     get
//     {
//         if (_stateChangedCommand == null)
//         {
//             _stateChangedCommand = new   RelayCommand<Windows.UI.Xaml.VisualStateChangedEventArgs>(args => GoToState(args.NewState.Name));
//         }

//         return _stateChangedCommand;
//     }
// }

// private void GoToState(string stateName)
// {
//     switch (stateName)
//     {
//         case PanoramicStateName:
//             DisplayMode = SplitViewDisplayMode.CompactInline;
//             break;
//         case WideStateName:
//             DisplayMode = SplitViewDisplayMode.CompactInline;
//             IsPaneOpen = false;
//             break;
//         case NarrowStateName:
//             DisplayMode = SplitViewDisplayMode.Overlay;
//             IsPaneOpen = false;
//             break;
//         default:
//             break;
//     }
// }

// private void InitializeState(double windowWith)
// {
//     if (windowWith < WideStateMinWindowWidth)
//     {
//         GoToState(NarrowStateName);
//     }
//     else if (windowWith < PanoramicStateMinWindowWidth)
//     {
//         GoToState(WideStateName);
//     }
//     else
//     {
//         GoToState(PanoramicStateName);
//     }
// }

public void Initialize(Frame frame)
{
    NavigationService.Frame = frame;
    NavigationService.Navigated += Frame_Navigated;
    PopulateNavItems();

    // Only remove the InitializeState call.
    // InitializeState(Window.Current.Bounds.Width);
}

private void ItemSelected(HamburgerMenuItemInvokedEventArgs args)
{
    // Only remove this part of the method.
    // if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
    // {
    //    IsPaneOpen = false;
    // }

    Navigate(args.InvokedItem);
}
```

Now you have to remove the adaptive triggers on each page that implements them (i.e. Blank Page).

**Remove the commented code**
```xml
<Page
    x:Class="App.Views.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    Style="{StaticResource PageStyle}"
    xmlns:fcu ="http://schemas.microsoft.com/winfx/2006/xaml/presentation?IsApiContractPresent(Windows.Foundation.UniversalApiContract,5)"
    xmlns:cu ="http://schemas.microsoft.com/winfx/2006/xaml/presentation?IsApiContractNotPresent(Windows.Foundation.UniversalApiContract,5)"
    mc:Ignorable="d">
    <Grid
        x:Name="ContentArea"
        Margin="{StaticResource MediumLeftRightMargin}">

        <Grid.RowDefinitions>
            <RowDefinition x:Name="TitleRow" Height="48"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <TextBlock
            x:Name="TitlePage"
            x:Uid="Main_Title"
            Style="{StaticResource PageTitleStyle}" />

        <Grid
            Grid.Row="1"
            Background="{ThemeResource SystemControlPageBackgroundChromeLowBrush}">
            <!--The SystemControlPageBackgroundChromeLowBrush background represents where you should place your content. 
                Place your content here.-->
        </Grid>
        <!--  Adaptive triggers  -->
        <!--
        <VisualStateManager.VisualStateGroups>
            <VisualStateGroup x:Name="WindowStates">
                <VisualState x:Name="WideState">
                    <VisualState.StateTriggers>
                        <AdaptiveTrigger MinWindowWidth="640"/>
                    </VisualState.StateTriggers>
                </VisualState>
                <VisualState x:Name="NarrowState">
                    <VisualState.StateTriggers>
                        <AdaptiveTrigger MinWindowWidth="0"/>
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Target="TitlePage.Margin" cu:Value="48,0,12,7" fcu:Value="0,0,12,7"/>
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateManager.VisualStateGroups>
        -->
    </Grid>
</Page>
```

The NavigationPane project type is based on the UWP Community Toolkit HamburgerMenu. This HamburgerMenu contains a property `UseNavigationViewWhenPossible` that allows it to use the NavigationView Control if the app is running on a device with Windows Fall Creators Update (10.16299) or higher.
There are several HamburgerMenu properties that have no effect when using the NavigationView. See the [HamburgerMenu documentation](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/docs/controls/HamburgerMenu.md) for more details.
