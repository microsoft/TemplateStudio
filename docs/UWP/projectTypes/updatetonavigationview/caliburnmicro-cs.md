# Update from HamburgerMenu to WinUI NavigationView in CaliburnMicro

If you have an UWP project created with WinTS with project type **NavigationPane** and framework **CaliburnMicro**  please follow these steps to update to WinUI NavigationView:

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

- `NavigationService NavigationFailed` and `Navigated` events handlers registration code inside `ActivateAsync` method.
- `OnFrameNavigated` method.
- Remove unused `using statements`.

The resulting code should look like this:

(Code in methods: `ActivateFromShareTargetAsync`, `InitializeAsync`, `StartupAsync` and `GetActivationHandlers` might change depending on the pages/features you used. `ActivateFromShareTargetAsync` will appears in ActivationService only if you have added ShareTarger feature.)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Caliburn.Micro;

using YourAppName.Activation;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace YourAppName.Services
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/activation.md
    internal class ActivationService
    {
        private readonly WinRTContainer _container;
        private readonly Type _defaultNavItem;
        private readonly Lazy<UIElement> _shell;

        public ActivationService(WinRTContainer container, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _container = container;
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
                    if (_shell?.Value == null)
                    {
                        var frame = new Frame();
                        NavigationService = _container.RegisterNavigationService(frame);
                        Window.Current.Content = frame;
                    }
                    else
                    {
                        var viewModel = ViewModelLocator.LocateForView(_shell.Value);

                        ViewModelBinder.Bind(viewModel, _shell.Value, null);

                        ScreenExtensions.TryActivate(viewModel);

                        NavigationService = _container.GetInstance<INavigationService>();
                        Window.Current.Content = _shell?.Value;
                    }
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
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem, NavigationService);
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

        private INavigationService NavigationService { get; set; }

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

### Thickness values to update

```xml
<Thickness x:Key="MediumLeftRightMargin">24,0,24,0</Thickness>
<Thickness x:Key="MediumLeftTopRightBottomMargin">24,24,24,24</Thickness>
```

### Thickness values to add

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
using Caliburn.Micro;

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

        public void Initialize(INavigationService navigationService)
        {
            navigationService.Navigated += OnNavigated;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            _current = this;
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

Add this extension class in the **Helpers** folder to the project. This allows the Windows UI NavigationViewItems to contain a Type property that is used for navigation.

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

### XAML code to remove

- xml namespaces for `fcu`, `cu`, `controls`, `ic` and `vm` (viewmodels).
- `NavigationMenuItemDataTemplate` DataTemplate in Page resources.
- `HamburgerMenu` control.

### XAML code to add

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
    x:Class="YourAppName.Views.ShellPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:i="using:Microsoft.Xaml.Interactivity"
    xmlns:cm="using:Caliburn.Micro"
    xmlns:winui="using:Microsoft.UI.Xaml.Controls"
    xmlns:behaviors="using:YourAppName.Behaviors"
    xmlns:helpers="using:YourAppName.Helpers"
    xmlns:views="using:YourAppName.Views"
    Background="{ThemeResource SystemControlBackgroundAltHighBrush}"
    mc:Ignorable="d">

    <winui:NavigationView
        x:Name="navigationView"
        IsBackButtonVisible="Visible"
        IsBackEnabled="{x:Bind ViewModel.IsBackEnabled, Mode=OneWay}"
        SelectedItem="{x:Bind ViewModel.Selected, Mode=OneWay}"
        IsSettingsVisible="True"
        ItemInvoked="OnItemInvoked"
        Background="{ThemeResource SystemControlBackgroundAltHighBrush}">
        <winui:NavigationView.MenuItems>

            <!-- All your pages -->
            <winui:NavigationViewItem x:Uid="Shell_Sample" Icon="Document" helpers:NavHelper.NavigateTo="views:SamplePage" />

        </winui:NavigationView.MenuItems>
        <i:Interaction.Behaviors>
            <behaviors:NavigationViewHeaderBehavior
                x:Name="navigationViewHeaderBehavior"
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
            <Frame x:Name="shellFrame" DataContext="{x:Null}" />
        </Grid>
    </winui:NavigationView>
</Page>
```

## 10. Changes in ShellPage.xaml.cs

### C# code to add (_Implementation below_)

- Add the following namespaces:

```xml
using Caliburn.Micro;
using Windows.UI.Xaml;
```

- Add `GetNavigationView` method.

### C# code to update (_Implementation below_)

- `CreateNavigationService` method implementation.

The resulting code should look like this:

```csharp
using Caliburn.Micro;
using YourAppName.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace YourAppName.Views
{
    public sealed partial class ShellPage : IShellView
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellPage()
        {
            InitializeComponent();
        }

        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            var navigationService = container.RegisterNavigationService(shellFrame);
            navigationViewHeaderBehavior.Initialize(navigationService);
            return navigationService;
        }

        public NavigationView GetNavigationView()
        {
            return navigationView;
        }

        private void OnItemInvoked(WinUI.NavigationView sender, WinUI.NavigationViewItemInvokedEventArgs args)
        {
            // Workaround for Issue https://github.com/Microsoft/WindowsTemplateStudio/issues/2774
            // Using EventTriggerBehavior does not work on WinUI NavigationView ItemInvoked event in Release mode.
            ViewModel.OnItemInvoked(args);
        }
    }
}
```

## 11. Changes in ShellViewModel.cs

ShellViewModel's complexity will be reduced significantly, these are the changes that you will have to implement on the class.

### C# code to remove

- private the following const properties: `Panoramic`, `Wide`, `Narrow`, `WideStateMinWindowWidth`, `PanoramicStateMinWindowWidth`.
- `_lastSelectedItem` private property.
- `_isPaneOpen` and `IsPaneOpen` properties.
- `_displayMode` and `DisplayMode` properties.
- ObservableCollections properties for `PrimaryItems` and `SecondaryItems`.
- `Open`, `StateChanged`, `GoToState`, `InitializeState`, `PopulateNavItems`, `ChangeSelected`, `Navigate` and `ItemSelected` methods.

- Remove unused using statements.

### C# code to add _(implementation below)_

- Add the following new usings statements:

```csharp
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml.Input;
using WinUI = Microsoft.UI.Xaml.Controls;
```

- Add `_navigationView`, `_altLeftKeyboardAccelerator`, `_backKeyboardAccelerator`, `_isBackEnabled` and `IsBackEnabled` members.
- `ItemInvokedCommand` and handler method `OnItemInvoked`.
- `IsMenuItemForPageType`, `BuildKeyboardAccelerator` and `OnKeyboardAcceleratorInvoked` methods.

### C# code to update _(implementation below)_

- Make `_navigationService` as static.
- `Selected` property DataType from `object` to `WinUI.NavigationViewItem`.
- `OnInitialize` and `NavigationService_Navigated` methods with the implementation below.

The resulting code should look like this:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Caliburn.Micro;

using YourAppName.Helpers;
using YourAppName.Views;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace YourAppName.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private readonly WinRTContainer _container;
        private static INavigationService _navigationService;
        private WinUI.NavigationView _navigationView;
        private bool _isBackEnabled;
        private WinUI.NavigationViewItem _selected;

        public ShellViewModel(WinRTContainer container)
        {
            _container = container;
        }

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

        protected override void OnInitialize()
        {
            var view = GetView() as IShellView;

            _navigationService = view?.CreateNavigationService(_container);
            _navigationView = view?.GetNavigationView();

            if (_navigationService != null)
            {
                _navigationService.Navigated += NavigationService_Navigated;
                _navigationView.BackRequested += OnBackRequested;
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            if (GetView() is UIElement page)
            {
                page.KeyboardAccelerators.Add(_altLeftKeyboardAccelerator);
                page.KeyboardAccelerators.Add(_backKeyboardAccelerator);
            }
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.Navigate(typeof(SettingsPage));
                return;
            }

            var item = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            var viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            _navigationService.NavigateToViewModel(viewModelType);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            _navigationService.GoBack();
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            Selected = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var sourceViewModelType = ViewModelLocator.LocateTypeForViewType(sourcePageType, false);
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            var viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            return viewModelType == sourceViewModelType;
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
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
                args.Handled = true;
            }
        }
    }
}
```

## 12. Changes in IShellView.cs

### XAML code to add _(implementation below)_

- Add `GetNavigationView` method.

```csharp
using System;

using Caliburn.Micro;
using Microsoft.UI.Xaml.Controls;

namespace YourAppName.Views
{
    public interface IShellView
    {
        INavigationService CreateNavigationService(WinRTContainer container);

        NavigationView GetNavigationView();
    }
}
```

## 13. Remove ShellNavigationItem.cs

ShellNavigationItem from ViewModels folder is no longer used and you should remove it from the project.

## 14. Update XAML code for all pages

The pages do no longer need the TitlePage TextBlock and the Adaptive triggers, because the page title will be displayed on the NavigationView HeaderTemplate and the responsive behaviors will be added by NavigationView control.

### XAML code to remove

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

- On **ShellViewModel.cs** go to **NavigationService_Navigated** method and add to the beginning:

```csharp
if (e.SourcePageType == typeof(SettingsPage))
{
    Selected = _navigationView.SettingsItem as NavigationViewItem;
    return;
}
```
