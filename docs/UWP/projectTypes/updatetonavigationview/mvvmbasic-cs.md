# Update from HamburgerMenu to WinUI NavigationView in MVVM Basic

If you have an UWP project created with WinTS with project type **NavigationPane** and framework **MVVM Basic**  please follow these steps to update to WinUI NavigationView:

## 1. Update target version in project properties

Windows UI library requires 17763 as target version in the project, to start using Windows UI in your project is necessary that you set 17763 as target version.

![Partial screenshot of project properties dialog showing targeting configuration](../../resources/project-types/cu-min-oct19-target.png)

## 2. Add the Nuget package reference

Add the Windows UI Library Nuget Package Reference (Microsoft.UI.Xaml):

![screenshot of NuGet Package Manager showing the 'Microsoft.UI.Xaml' package](../../resources/project-types/winui-nugetpackage.png)

## 3. Changes in App.xaml

Add the WinUI Xaml Resources dictionary to the MergedDictionaries:

```xml
<ResourceDictionary.MergedDictionaries>

    <!--Add WinUI resources dictionary-->
    <XamlControlsResources  xmlns="using:Microsoft.UI.Xaml.Controls"/>
    <!-- ··· -->
    <!--Other resources dictionaries-->

</ResourceDictionary.MergedDictionaries>
```

## 4. Changes in ActivationService.cs

Remove the code to manage back navigation from ActivationService, this code will later be added to the ShellPage.

### C# code you will have to remove:

- `SystemNavigationManager BackRequested` and `NavigationService NavigationFailed` and `Navigated` events handlers registration code inside `ActivateAsync` method.
- `ActivationService_BackRequested` and `Frame_Navigated` methods.
- Remove unused `using statements`.

The resulting code should look like this:

(Code in methods: `ActivateFromShareTargetAsync`, `InitializeAsync`, `StartupAsync` and `GetActivationHandlers` might change depending on the pages/features you used. `ActivateFromShareTargetAsync` will appears in ActivationService only if you have added ShareTarger feature.)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using YourAppName.Activation;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace YourAppName.Services
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/activation.md
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Lazy<UIElement> _shell;
        private readonly Type _defaultNavItem;

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize things like registering background task before the app is loaded
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = _shell?.Value ?? new Frame();
                }
            }

            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }

                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }

        private async Task InitializeAsync()
        {
            // Here your Initialize actions
        }

        private async Task StartupAsync()
        {
            // Here your StartUp actions
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            // Here your ActivationHandlers
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }
    }
}
```

## 5. Changes in _Thickness.xaml

Update and add new Margins that will be used in pages.

### Thickness values you will have to update.

```xml
<Thickness x:Key="MediumLeftRightMargin">24,0,24,0</Thickness>
<Thickness x:Key="MediumLeftTopRightBottomMargin">24,24,24,24</Thickness>
```

### Thickness values you will have to add.

```xml
<!--Medium size margins-->
<Thickness x:Key="MediumTopMargin">0,24,0,0</Thickness>
<Thickness x:Key="MediumBottomMargin">0,0,24,0</Thickness>

<!--Small size margins-->
<Thickness x:Key="SmallLeftMargin">12, 0, 0, 0</Thickness>
<Thickness x:Key="SmallTopMargin">0, 12, 0, 0</Thickness>
<Thickness x:Key="SmallTopBottomMargin">0, 12, 0, 12</Thickness>
<Thickness x:Key="SmallLeftRightMargin">12, 0, 12, 0</Thickness>

<!--Extra Small size margins-->
<Thickness x:Key="ExtraSmallTopMargin">0, 8, 0, 0</Thickness>
```

## 6. Add NavigationViewHeaderBehavior.cs

This behavior allows the NavigationView to hide or customize the NavigationViewHeader depending on the page that is shown, you can read more about this behavior [here](../navigationpane.md). Add the following NavigationViewHeaderBehavior class to the Behaviors folder, if your solution doesn't have a Behaviors folder you will have to add it.

```csharp
using YourAppName.Services;

using Microsoft.Xaml.Interactivity;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace YourAppName.Behaviors
{
    public class NavigationViewHeaderBehavior : Behavior<WinUI.NavigationView>
    {
        private static NavigationViewHeaderBehavior _current;
        private Page _currentPage;

        public DataTemplate DefaultHeaderTemplate { get; set; }

        public object DefaultHeader
        {
            get { return GetValue(DefaultHeaderProperty); }
            set { SetValue(DefaultHeaderProperty, value); }
        }

        public static readonly DependencyProperty DefaultHeaderProperty = DependencyProperty.Register("DefaultHeader", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current.UpdateHeader()));

        public static NavigationViewHeaderMode GetHeaderMode(Page item)
        {
            return (NavigationViewHeaderMode)item.GetValue(HeaderModeProperty);
        }

        public static void SetHeaderMode(Page item, NavigationViewHeaderMode value)
        {
            item.SetValue(HeaderModeProperty, value);
        }

        public static readonly DependencyProperty HeaderModeProperty =
            DependencyProperty.RegisterAttached("HeaderMode", typeof(bool), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(NavigationViewHeaderMode.Always, (d, e) => _current.UpdateHeader()));

        public static object GetHeaderContext(Page item)
        {
            return item.GetValue(HeaderContextProperty);
        }

        public static void SetHeaderContext(Page item, object value)
        {
            item.SetValue(HeaderContextProperty, value);
        }

        public static readonly DependencyProperty HeaderContextProperty =
            DependencyProperty.RegisterAttached("HeaderContext", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current.UpdateHeader()));

        public static DataTemplate GetHeaderTemplate(Page item)
        {
            return (DataTemplate)item.GetValue(HeaderTemplateProperty);
        }

        public static void SetHeaderTemplate(Page item, DataTemplate value)
        {
            item.SetValue(HeaderTemplateProperty, value);
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current.UpdateHeaderTemplate()));

        protected override void OnAttached()
        {
            base.OnAttached();
            _current = this;
            NavigationService.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var frame = sender as Frame;
            if (frame.Content is Page page)
            {
                _currentPage = page;

                UpdateHeader();
                UpdateHeaderTemplate();
            }
        }

        private void UpdateHeader()
        {
            if (_currentPage != null)
            {
                var headerMode = GetHeaderMode(_currentPage);
                if (headerMode == NavigationViewHeaderMode.Never)
                {
                    AssociatedObject.Header = null;
                    AssociatedObject.AlwaysShowHeader = false;
                }
                else
                {
                    var headerFromPage = GetHeaderContext(_currentPage);
                    if (headerFromPage != null)
                    {
                        AssociatedObject.Header = headerFromPage;
                    }
                    else
                    {
                        AssociatedObject.Header = DefaultHeader;
                    }

                    if (headerMode == NavigationViewHeaderMode.Always)
                    {
                        AssociatedObject.AlwaysShowHeader = true;
                    }
                    else
                    {
                        AssociatedObject.AlwaysShowHeader = false;
                    }
                }
            }
        }

        private void UpdateHeaderTemplate()
        {
            if (_currentPage != null)
            {
                var headerTemplate = GetHeaderTemplate(_currentPage);
                AssociatedObject.HeaderTemplate = headerTemplate ?? DefaultHeaderTemplate;
            }
        }
    }
}
```

## 7. Add NavigationViewHeaderMode.cs

Add the NavigationViewHeaderBehavior enum to the Behaviors folder.

```csharp
namespace YourAppName.Behaviors
{
    public enum NavigationViewHeaderMode
    {
        Always,
        Never,
        Minimal
    }
}
```

## 8. Add NavHelper.cs

Add this extension class to the **Helpers** folder to the project. This allows the Windows UI NavigationViewItems to contain a Type property that is used for navigation.

```csharp
using System;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace YourAppName.Helpers
{
    public class NavHelper
    {
        public static Type GetNavigateTo(NavigationViewItem item)
        {
            return (Type)item.GetValue(NavigateToProperty);
        }

        public static void SetNavigateTo(NavigationViewItem item, Type value)
        {
            item.SetValue(NavigateToProperty, value);
        }

        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", typeof(Type), typeof(NavHelper), new PropertyMetadata(null));
    }
}
```

## 9. Changes in ShellPage.xaml

The updated ShellPage will include the WinUI NavigationView and add the MenuItems directly in Xaml. The NavigationViewItems include an extension property that contains the target page type to navigate in the frame.

### XAML code you will have to remove:

- xml namespaces for `fcu`, `cu`, `controls` and `vm` (viewmodels).
- `NavigationMenuItemDataTemplate` DataTemplate in Page resources.
- `HamburgerMenu` control.

### XAML code you will have to add:

- The following xml namespaces:

```xml
xmlns:winui="using:Microsoft.UI.Xaml.Controls"
xmlns:behaviors="using:YourAppName.Behaviors"
xmlns:helpers="using:YourAppName.Helpers"
xmlns:views="using:YourAppName.Views"
```

- `winui:NavigationView` control.
- `winui:NavigationViewItem` MenuItems inside of the `winui:NavigationView`.
- `NavigationViewHeaderBehavior` behavior inside of the `winui:NavigationView`.

The resulting code should look like this:

```xml
<Page
    x:Class="SampleApp.Views.ShellPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:winui="using:Microsoft.UI.Xaml.Controls"
    xmlns:behaviors="using:YourAppName.Behaviors"
    xmlns:helpers="using:YourAppName.Helpers"
    xmlns:views="using:YourAppName.Views"
    xmlns:ic="using:Microsoft.Xaml.Interactions.Core"
    xmlns:i="using:Microsoft.Xaml.Interactivity"
    mc:Ignorable="d">

    <i:Interaction.Behaviors>
        <ic:EventTriggerBehavior EventName="Loaded">
            <ic:InvokeCommandAction Command="{x:Bind ViewModel.LoadedCommand}" />
        </ic:EventTriggerBehavior>
    </i:Interaction.Behaviors>


    <winui:NavigationView
        x:Name="navigationView"
        IsBackButtonVisible="Visible"
        IsBackEnabled="{x:Bind ViewModel.IsBackEnabled, Mode=OneWay}"
        SelectedItem="{x:Bind ViewModel.Selected, Mode=OneWay}"
        ItemInvoked="OnItemInvoked"
        Background="{ThemeResource SystemControlBackgroundAltHighBrush}">
        <winui:NavigationView.MenuItems>

            <!-- All your pages -->
            <winui:NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavHelper.NavigateTo="views:MainPage" />

        </winui:NavigationView.MenuItems>
        <i:Interaction.Behaviors>
            <behaviors:NavigationViewHeaderBehavior
                DefaultHeader="{x:Bind ViewModel.Selected.Content, Mode=OneWay}">
                <behaviors:NavigationViewHeaderBehavior.DefaultHeaderTemplate>
                    <DataTemplate>
                        <Grid>
                            <TextBlock
                                Text="{Binding}"
                                Style="{ThemeResource TitleTextBlockStyle}"
                                Margin="{StaticResource SmallLeftRightMargin}" />
                        </Grid>
                    </DataTemplate>
                </behaviors:NavigationViewHeaderBehavior.DefaultHeaderTemplate>
            </behaviors:NavigationViewHeaderBehavior>
        </i:Interaction.Behaviors>
        <Grid>
            <Frame x:Name="shellFrame" />
        </Grid>
    </winui:NavigationView>
</Page>
```

## 10. Changes in ShellPage.xaml.cs

ShellViewModel will need the WinUI `NavigationView` instance and `KeyboardAccelerators` (explained below), you will have to add it on initialization.

### C# code you will have to update (_Implementation below_):

- Add the `navigationView` control and `KeyboardAccelerators` to ViewModel `Initialize` call.

The resulting code should look like this:

```csharp
using System;
using YourAppName.ViewModels;
using Windows.UI.Xaml.Controls;

namespace YourAppName.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
        }

        private void OnItemInvoked(WinUI.NavigationView sender, WinUI.NavigationViewItemInvokedEventArgs args)
        {
            // Workaround for Issue https://github.com/Microsoft/WindowsTemplateStudio/issues/2774
            // Using EventTriggerBehavior does not work on WinUI NavigationView ItemInvoked event in Release mode.
            ViewModel.ItemInvokedCommand.Execute(args);
        }
    }
}
```

## 11. Changes in ShellViewModel.cs

ShellViewModel's complexity will be reduced significantly, these are the changes that you will have to implement on the class.

### C# code you will have to remove:

- private the following const properties: `Panoramic`, `Wide`, `Narrow`, `WideStateMinWindowWidth`, `PanoramicStateMinWindowWidth`.
- private fields `_lastSelectedItem` and `_itemInvokedCommand`
- `IsPaneOpen` observable property.
- `DisplayMode` observable property.
- ObservableCollections properties for `PrimaryItems` and `SecondaryItems`.
- `OpenPaneCommand`.
- `ItemSelectedCommand` and handler method `ItemSelected`.
- `StateChangedCommand` and handler method `GoToState`.
- `Navigate` and `ChangeSelected` methods.
- Remove unused using statements.

### C# code you will have to add _(implementation below)_:

- Add the following new usings statements:

```csharp
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml.Input;
using WinUI = Microsoft.UI.Xaml.Controls;
```

- Add `_keyboardAccelerators`, `_navigationView`, `AltLeftKeyboardAccelerator`, `BackKeyboardAccelerator`, `_loadedCommand`, `LoadedCommand` and `IsBackEnabled` members.
- `ItemInvokedCommand` and handler method `OnItemInvoked`.
- `OnLoaded`, `IsMenuItemForPageType`, `BuildKeyboardAccelerator` and `OnKeyboardAcceleratorInvoked` methods.

### C# code you will have to update _(implementation below)_:

- `Selected` property DataType from `object` to `WinUI.NavigationViewItem`.
- `Initialize` method with the implementation below.
- `Frame_Navigated` method with the implementation below.

The resulting code should look like this:

```csharp
public class ShellViewModel : Observable
{
    private bool _isBackEnabled;
    private WinUI.NavigationView _navigationView;
    private WinUI.NavigationViewItem _selected;
    private ICommand _itemInvokedCommand;
    private ICommand _loadedCommand;
    private IList<KeyboardAccelerator> _keyboardAccelerators;

    public bool IsBackEnabled
    {
        get { return _isBackEnabled; }
        set { Set(ref _isBackEnabled, value); }
    }

    public WinUI.NavigationViewItem Selected
    {
        get { return _selected; }
        set { Set(ref _selected, value); }
    }

    public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));

    public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

    public readonly KeyboardAccelerator AltLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);

    public readonly KeyboardAccelerator BackKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

    public ShellViewModel()
    {
    }

    public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
    {
        _navigationView = navigationView;
        NavigationService.Frame = frame;
        NavigationService.Navigated += Frame_Navigated;
        _navigationView.BackRequested += OnBackRequested;
        _keyboardAccelerators = keyboardAccelerators;
    }

    private void OnLoaded()
    {
        _keyboardAccelerators.Add(_altLeftKeyboardAccelerator);
        _keyboardAccelerators.Add(_backKeyboardAccelerator);
    }

    private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
    {
        NavigationService.GoBack();
    }

    private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
    {
        var item = _navigationView.MenuItems
                        .OfType<WinUI.NavigationViewItem>()
                        .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
        var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
        NavigationService.Navigate(pageType);
    }

    private void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;
        Selected = _navigationView.MenuItems
                        .OfType<WinUI.NavigationViewItem>()
                        .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
    }

    private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
    {
        var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
        return pageType == sourcePageType;
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var result = NavigationService.GoBack();
        args.Handled = result;
    }
}
```

## 12. Changes in NavigationService

### C# code you will have to update:

- GoBack method implementation.

The resulting method code should look like this:

```csharp
public static bool GoBack()
{
    if (CanGoBack)
    {
        Frame.GoBack();
        return true;
    }

    return false;
}
```

## 13. Remove ShellNavigationItem.cs

ShellNavigationItem from ViewModels folder is no longer used and you should remove it from the project.

## 14. Update XAML code for all pages

The pages do no longer need the TitlePage TextBlock and the Adaptive triggers, because the page title will be displayed on the NavigationView HeaderTemplate and the responsive behaviors will be added by NavigationView control.

### XAML code you will have to remove:

- **xmln namespaces** for fcu and cu.
- Textblock **TitlePage**
- ContentArea Grid **RowDefinitions**
- VisualStateManager **VisualStateGroups**.
- **Grid.Row="1"** property  in the content Grid.

The resulting code should look like this:

```xml
<Page
    x:Class="SampleApp.Views.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    Style="{StaticResource PageStyle}"
    mc:Ignorable="d">
    <Grid
        x:Name="ContentArea"
        Margin="{StaticResource MediumLeftRightMargin}">
        <Grid
            Background="{ThemeResource SystemControlPageBackgroundChromeLowBrush}">
            <!--The SystemControlPageBackgroundChromeLowBrush background represents where you should place your content.
                Place your content here.-->
        </Grid>
    </Grid>
</Page>
```

## 15. Update Navigation View item name for all pages in Resources.resw

As NavigationItems and their names are defined in xaml now, you need to add `.Content` to each of the navigation view item names.
(_for example `Shell_Main` should be changed to `Shell_Main.Content`_)

## 16. Settings Page

If your project contains a SettingsPage you must perform the following steps:

- On **ShellPage.xaml** change **IsSettingsVisible** property to true.
- On **ShellViewModel.cs** go to **OnItemInvoked** method and add to the beginning:

```csharp
if (args.IsSettingsInvoked)
{
    NavigationService.Navigate(typeof(SettingsPage));
    return;
}
```

- On **ShellViewModel.cs** go to **Frame_Navigated** method and add to the beginning:

```csharp
if (e.SourcePageType == typeof(SettingsPage))
{
    Selected = _navigationView.SettingsItem as NavigationViewItem;
    return;
}
```
