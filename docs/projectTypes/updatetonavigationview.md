# Update from HamburgerMenu to NavigationView
If you have an UWP project created with WTS with the project type **NavigationPane** and you want to update to NavigationView, please follow these steps:

## 1. Update min target version in project properties
NavigationView is a Fall Creators Update control, to start using it in your project is neccessary that  you set FCU as min version.
![](../resources/project-types/fcu-min-version.png)

## 2. Update ShellPage.xaml
The updated ShellPage will include the NavigationView and add the MenuItems directly in Xaml. The NavigationViewItems include an extension property that contains the target page type to navigate in the frame.

### XAML code you will have to remove:
 - **xmln namespaces** for fcu and cu.
 - DataTemplate **NavigationMenuItemDataTemplate** in Page resources.
 - **HamburgerMenu** control.
  - **VisualStateGroups** at the bottom of the page's main grid.

### XAML code you will have to add:
 - **NavigationView** control.
 - **MenuItems** inside of the NavigationView.
 - **HeaderTemplate** inside of the NavigationView.

 The resulting code should look like this:

```xml
<Page
    x:Class="SampleApp.Views.ShellPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:helpers="using:SampleApp.Helpers"
    xmlns:views="using:SampleApp.Views"
    xmlns:ic="using:Microsoft.Xaml.Interactions.Core"
    xmlns:i="using:Microsoft.Xaml.Interactivity"
    mc:Ignorable="d">

    <NavigationView
        x:Name="navigationView"
        SelectedItem="{x:Bind ViewModel.Selected, Mode=OneWay}"
        Header="{Binding Selected.Title}"
        IsSettingsVisible="True"
        Background="{ThemeResource SystemControlBackgroundAltHighBrush}">
        <NavigationView.MenuItems>
            <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavigationViewItemExtensions.PageType="views:MainPage" />
            <!--
            Add here other menu item pages
            -->
        </NavigationView.MenuItems>
        <NavigationView.HeaderTemplate>
            <DataTemplate>
                <TextBlock
                    Style="{StaticResource TitleTextBlockStyle}"
                    Margin="12,0,0,0"
                    VerticalAlignment="Center"
                    Text="{Binding Selected.Content}" />
            </DataTemplate>
        </NavigationView.HeaderTemplate>
        <i:Interaction.Behaviors>
            <ic:EventTriggerBehavior EventName="ItemInvoked">
                <ic:InvokeCommandAction Command="{x:Bind ViewModel.ItemInvokedCommand}" />
            </ic:EventTriggerBehavior>
        </i:Interaction.Behaviors>
        <Grid>
            <Frame x:Name="shellFrame" />
        </Grid>
    </NavigationView>
</Page>
```
## 3. Update ShellPage.xaml.cs
ShellViewModel will need the NavigationView instance (explained below), you will have to add it on initialization.

### C# code you will have to modify:
 - Add the navigationView control to ViewModel initialization.

The resulting code should look like this:
 ```csharp
public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel { get; } = new ShellViewModel();

    public ShellPage()
    {
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.Initialize(shellFrame, navigationView);
    }
}
```

## 4. Add NavigationViewItemExtensions.cs

Add this extension class in the **Helpers** folder to the project. This allows the NavigationViewItems to contain a Type property that is used for navigation.
```csharp
public class NavigationViewItemExtensions
{
    public static Type GetPageType(NavigationViewItem obj)
    {
        return (Type)obj.GetValue(PageTypeProperty);
    }

    public static void SetPageType(NavigationViewItem obj, Type value)
    {
        obj.SetValue(PageTypeProperty, value);
    }

    public static readonly DependencyProperty PageTypeProperty =
        DependencyProperty.RegisterAttached("PageType", typeof(Type),
        typeof(NavigationViewItemExtensions), new PropertyMetadata(null));
}
```

## 5. Update ShellViewModel.cs
ShellViewModel's complexity will be reduced significantly, these are the changes that you will have to implement on the class.
### C# code you will have to remove:
 - private **const properties** for Visual States (Panoramic, Wide, Narrow).
 - **IsPaneOpen** observable property.
 - **DisplayMode** observable property.
 - **ObservableCollections** properties for **PrimaryItems** and **SecondaryItems**.
 - **OpenPaneCommand** and handler method.
 - **ItemSelectedCommand** and handler method.
 - **StateChangedCommand** and handler method.
 - **GoToState** method.
 - **PopulateNavItems** method and method call from Initialize.

### C# code you will have to add _(implementation below)_:
 - **ItemInvokedCommand** and handler method.
  - **IsNavigationViewItemFromPageType** method.

### C# code you will have to update _(implementation below)_:
 - **Initialize** method.
 - **Frame_Navigated** method with the implementation below.

 The resulting code should look like this:
```csharp
public class ShellViewModel : Observable
{
    private NavigationView _navigationView;
    private object _selected;
    private ICommand _itemInvokedCommand;

    public object Selected
    {
        get { return _selected; }
        set { Set(ref _selected, value); }
    }

    public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked));

    public ShellViewModel()
    {
    }

    public void Initialize(Frame frame, NavigationView navigationView)
    {
        _navigationView = navigationView;
        NavigationService.Frame = frame;
        NavigationService.Navigated += Frame_Navigated;
    }

    private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            NavigationService.Navigate(typeof(SettingsPage));
            return;
        }
        var item = _navigationView.MenuItems
                        .OfType<NavigationViewItem>()
                        .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
        var pageType = item.GetValue(NavigationViewItemExtensions.PageTypeProperty) as Type;
        NavigationService.Navigate(pageType);
    }

    private void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = _navigationView.SettingsItem;
            return;
        }
        Selected = _navigationView.MenuItems
                        .OfType<NavigationViewItem>()
                        .First(menuItem => IsNavigationViewItemFromPageType(menuItem, e.SourcePageType));
    }

    private bool IsNavigationViewItemFromPageType(NavigationViewItem menuItem, Type sourcePageType)
    {
        var pageType = menuItem.GetValue(NavigationViewItemExtensions.PageTypeProperty) as Type;
        return pageType == sourcePageType;
    }
}
```

## 6. Remove ShellNavigationItem.cs
ShellNavigationItem is no longer used and you should remove it from the project.

## 7. Update XAML code for all pages
The pages do no longer need the TitlePage TextBlock and the Adaptive triggers, because the page title will be displayed on the NavigationView HeaderTemplate and the responsive behaviors will be added by NavigationView control.

### XAML code you will have to remove:
 - Main Grid **RowDefinitions**
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