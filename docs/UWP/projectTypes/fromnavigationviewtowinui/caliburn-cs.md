# Update NavigationView to WinUI in MVVMBasic apps

If you have an UWP project created with WinTS with project type **NavigationPane** and framework **CaliburnMicro** please follow these steps to update from NavigationView to Windows UI NavigationView:

## 1. Update target version in project properties

Windows UI library requires 17763 as target version in the project, to start using Windows UI in your project is necessary that you set 17763 as target version.

![Partial screenshot of project properties dialog showing targeting configuration](../../resources/project-types/fu-min-oct19-target.png)

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

- `KeyboardAccelerator` static members.
- `BuildKeyboardAccelerator`, `OnKeyboardAcceleratorInvoked` and `OnFrameNavigated` methods.
- `NavigationService NavigationFailed` and `Navigated` events handlers registration code inside `ActivateAsync` method.
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

## 8. Changes in NavHelper.cs and IShellView.cs

Adjust using statement to move the NavigationView and NavigationViewItem data types to Windows UI data types.

### Change the using statement

From

`Windows.UI.Xaml.Controls`

To

`Microsoft.UI.Xaml.Controls`

## 9. Changes in ShellPage.xaml

The updated ShellPage will contain a WinUI NavigationView that handles back navigation in the app using the NavigationView's BackButton and the above mentioned bahavior to hide/personalize the NavViewHeader depending on the page shown.

### Xaml code you will have to add (_Implementation below_):

- `i`, `ic`, `winui` and `behaviors` namespaces in page declaration.
- Add `IsBackButtonVisible` and `IsBackEnabled` properties to NavigationView.
- Add `NavigationViewHeaderBehavior` with `DefaultHeader` and `DefaultHeaderTemplate` properties to NavigationView behaviors.

### Xaml code you will have to update (_Implementation below_):

- Add the `winui:` namespace to `NavigationView` and `NavigationViewItems` data types.

### Xaml code you will have to remove:

- `Header` and `HeaderTemplate` properties from NavigationView.

The resulting code should look like this:

```xml
<Page
    x:Class="YourAppName.Views.ShellPage"
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
            <Frame x:Name="shellFrame" />
        </Grid>
    </winui:NavigationView>
</Page>
```

## 10. Changes in ShellPage.xaml.cs

### C# code you will have to add (_Implementation below_):

- Add the following using statement:

```csharp
using WinUI = Microsoft.UI.Xaml.Controls;
```

- Add `WinUI.` namespace alias to `NavigationView` Data Type.

### C# code you will have to remove:

- Remove `HideNavViewBackButton` method.
- Remove from the page constructor `HideNavViewBackButton` call.
- Remove from the page constructor `KeyboardAccelerators` additions.
- Remove unused using statements.

### C# code you will have to update (_Implementation below_):

- Add `KeyboardAccelerators` collection to ViewModel Initialize call.
- Call to `navigationViewHeaderBehavior` `Initialize` from CreateNavigationService with `navigationService` as parameter

The resulting code should look like this:

```csharp
using Caliburn.Micro;
using YourAppName.ViewModels;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace YourAppName.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
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

        public WinUI.NavigationView GetNavigationView()
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

### C# code you will have to add (_Implementation below_):

- Add the following new usings statements:

```csharp
using Windows.System;
using Windows.UI.Xaml.Input;
using WinUI = Microsoft.UI.Xaml.Controls;
```

- Add `WinUI.` namespace alias to `NavigationView`, `NavigationViewItem` and `NavigationViewItemInvokedEventArgs` Data Types.
- Add `_altLeftKeyboardAccelerator`, `_backKeyboardAccelerator` and `IsBackEnabled` members.
- Add `_altLeftKeyboardAccelerator` and `_backKeyboardAccelerator` to page in OnInitialize method.
- Add `BuildKeyboardAccelerator`, `OnKeyboardAcceleratorInvoked` and `OnBackRequested` methods.
- Subscribe to `BackRequested` event handler in Initialize.
- Set `IsBackEnabled` to `NavigationService.CanGoBack` at the begining of `Frame_Navigated` method.

### C# code you will have to update (_Implementation below_):

- Make _navigationService as static.

### C# code you will have to remove:

- Remove unused using statements.

The resulting code should look like this:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using YourAppName.Helpers;
using YourAppName.Services;
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
