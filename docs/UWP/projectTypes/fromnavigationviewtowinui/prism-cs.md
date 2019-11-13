# Update NavigationView to WinUI in MVVMBasic apps

If you have an UWP project created with WinTS with project type **NavigationPane** and framework **Prism**  please follow these steps to update from NavigationView to Windows UI NavigationView:

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

## 4. Changes in _Thickness.xaml

Update and add new Margins that will be used in pages.

### Thickness values you will have to update

```xml
<Thickness x:Key="MediumLeftRightMargin">24,0,24,0</Thickness>
<Thickness x:Key="MediumLeftTopRightBottomMargin">24,24,24,24</Thickness>
```

### Thickness values you will have to add

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

## 5. Add NavigationViewHeaderBehavior.cs

This behavior allows the NavigationView to hide or customize the NavigationViewHeader depending on the page that is shown, you can read more about this behavior [here](../navigationpane.md). Add the following NavigationViewHeaderBehavior class to the Behaviors folder, if your solution doesn't have a Behaviors folder you will have to add it.

```csharp
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

        public void Initialize(Frame frame)
        {
            frame.Navigated += OnNavigated;
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

## 6. Add NavigationViewHeaderMode.cs

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

## 7. Changes in NavHelper.cs

Adjust the using statement to move the NavigationViewItem properties to Windows UI NavigationView.

### Change the using statement

From

`Windows.UI.Xaml.Controls`

To

`Microsoft.UI.Xaml.Controls`

## 8. Changes in ShellPage.xaml

The updated ShellPage will contain a WinUI NavigationView that handles back navigation in the app using the NavigationView's BackButton and the above mentioned behavior to hide/personalize the NavViewHeader depending on the page shown.

### Xaml code you will have to add (_Implementation below_):

- `winui` and `behaviors` namespaces in page declaration.
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
    xmlns:prismMvvm="using:Prism.Windows.Mvvm"
    prismMvvm:ViewModelLocator.AutoWireViewModel="True"
    xmlns:winui="using:Microsoft.UI.Xaml.Controls"
    xmlns:behaviors="using:YourAppName.Behaviors"
    xmlns:i="using:Microsoft.Xaml.Interactivity"
    xmlns:ic="using:Microsoft.Xaml.Interactions.Core"
    xmlns:helpers="using:YourAppName.Helpers"
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
            <winui:NavigationViewItem x:Uid="Shell_Sample" Icon="Document" helpers:NavHelper.NavigateTo="Sample" />

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

## 9. Changes in ShellPage.xaml.cs

### C# code you will have to remove:

- Remove `HideNavViewBackButton` method.
- Remove from the page constructor `HideNavViewBackButton` call.
- Remove unused using statements.

### C# code you will have to add (_Implementation below_):

- Call `navigationViewHeaderBehavior` `Initialize` with `frame` as parameter from `SetRootFrame` method.

The resulting code should look like this:

```csharp
using System;
using YourAppName.ViewModels;
using Windows.UI.Xaml.Controls;

namespace YourAppName.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public Frame ShellFrame => shellFrame;

        public ShellPage()
        {
            InitializeComponent();
        }

        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
            navigationViewHeaderBehavior.Initialize(frame);
            ViewModel.Initialize(frame, navigationView);
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

## 10. Changes in ShellViewModel.cs

### C# code you will have to add (_Implementation below_):

- Add the following new usings statements:

```csharp
using WinUI = Microsoft.UI.Xaml.Controls;
```

- Add `WinUI.` namespace alias to `NavigationView`, `NavigationViewItem` and `NavigationViewItemInvokedEventArgs` Data Types.
- Add `OnBackRequested` method.
- Subscribe to `BackRequested` event handler in Initialize.
- Set `IsBackEnabled` to `NavigationService.CanGoBack` at the begining of `Frame_Navigated` method.

The resulting code should look like this:

```csharp
using System;
using System.Linq;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using YourAppName.Helpers;
using YourAppName.Views;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace YourAppName.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private bool _isBackEnabled;
        private WinUI.NavigationView _navigationView;
        private WinUI.NavigationViewItem _selected;

        public ICommand ItemInvokedCommand { get; }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ShellViewModel(INavigationService navigationServiceInstance)
        {
            _navigationService = navigationServiceInstance;
            ItemInvokedCommand = new DelegateCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView)
        {
            _navigationView = navigationView;
            frame.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.Navigate("Settings", null);
                return;
            }

            var item = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;
            _navigationService.Navigate(pageKey, null);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            _navigationService.GoBack();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack();
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
            var sourcePageKey = sourcePageType.Name;
            sourcePageKey = sourcePageKey.Substring(0, sourcePageKey.Length - 4);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            return pageKey == sourcePageKey;
        }
    }
}
```
