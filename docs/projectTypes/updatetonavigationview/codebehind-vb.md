# Update from HamburgerMenu to NavigationView in Code Behind
If you have an UWP project created with WTS with project type **NavigationPane** and framework **Code Behind**  please follow these steps to update to NavigationView:

## 1. Update min target version in project properties
NavigationView is a Fall Creators Update control, to start using it in your project is neccessary that you set FCU as min version.
![](../../resources/project-types/fcu-min-version.png)

## 2. Update ShellPage.xaml
The updated ShellPage will include the NavigationView and add the MenuItems directly in Xaml. The NavigationViewItems include an extension property that contains the target page type to navigate in the frame.

### XAML code you will have to remove:
 - **xmln namespaces** for fcu and cu.
 - DataTemplate **NavigationMenuItemDataTemplate** in Page resources.
 - **HamburgerMenu** control.

### XAML code you will have to add:
 - **namespaces**: xmlns:helpers="using:myAppNamespace.Helpers"
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
    mc:Ignorable="d">

    <NavigationView
        x:Name="navigationView"
        SelectedItem="{x:Bind Selected, Mode=OneWay}"
        Header="{x:Bind Selected.Content, Mode=OneWay}"
        ItemInvoked="OnItemInvoked"
        IsSettingsVisible="False"
        Background="{ThemeResource SystemControlBackgroundAltHighBrush}">
        <NavigationView.MenuItems>
            <!--
            TODO WTS: Change the symbols for each item as appropriate for your app
            More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/navigationpane.md
            Edit String/en-US/Resources.resw: Add a menu item title for each page
            -->
            <NavigationViewItem x:Uid="Shell_Main" Icon="Document"  helpers:NavHelper.NavigateTo="views:MainPage" />
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
                    Text="{Binding}" />
            </DataTemplate>
        </NavigationView.HeaderTemplate>
        <Grid>
            <Frame x:Name="shellFrame" />
        </Grid>
    </NavigationView>
</Page>
```
## 3. Update ShellPage.xaml.cs
ShellPage's codebehind complexity will be reduced significantly, these are the changes that you will have to implement on the class.

### VB code you will have to remove:
 - private **const properties** for Visual States (Panoramic, Wide, Narrow).
 - private field **_lastSelectedItem**
 - **IsPaneOpen** observable property.
 - **DisplayMode** observable property.
 - **ObservableCollections** properties for **PrimaryItems** and **SecondaryItems**.
 - **ItemInvoked** event handler.
 - **OpenPane_Click** event handler.
 - **WindowStates_CurrentStateChanged** event handler.
 - **GoToState**, **ChangeSelected** and **Navigate** method.
 - **PopulateNavItems** method and method call from Initialize.
 - **InitializeState** method and method call from Initialize.

 ### VB code you will have to add _(implementation below)_:
 - **OnItemInvoked** event handler.
  - **IsMenuItemForPageType** method.

### VB code you will have to update _(implementation below)_:
 - **Frame_Navigated** method with the implementation below.

The resulting code should look like this:
 ```vbnet
Partial Public NotInheritable Class ShellPage
    Inherits Page
    Implements INotifyPropertyChanged

    Private _selected As NavigationViewItem

    Public Property Selected As NavigationViewItem
        Get
            Return _selected
        End Get

        Set(value As NavigationViewItem)
            [Set](_selected, value)
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
        HideNavViewBackButton()
        DataContext = Me
        Initialize()
    End Sub

    Private Sub Initialize()
        NavigationService.Frame = shellFrame
        AddHandler NavigationService.Navigated, AddressOf Frame_Navigated
    End Sub

    Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
        Selected = navigationView.MenuItems.OfType(Of NavigationViewItem)().FirstOrDefault(Function(menuItem) IsMenuItemForPageType(menuItem, e.SourcePageType))
    End Sub

    Private Function IsMenuItemForPageType(menuItem As NavigationViewItem, sourcePageType As Type) As Boolean
        Dim pageType = TryCast(menuItem.GetValue(NavHelper.NavigateToProperty), Type)
        Return pageType = sourcePageType
    End Function

    Private Sub OnItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)
        Dim item = navigationView.MenuItems.OfType(Of NavigationViewItem)().First(Function(menuItem) CStr(menuItem.Content) = CStr(args.InvokedItem))
        Dim pageType = TryCast(item.GetValue(NavHelper.NavigateToProperty), Type)
        NavigationService.Navigate(pageType)
    End Sub

    Private Sub HideNavViewBackButton()
        If ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6) Then
            navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed
        End if
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
        If Equals(storage, value) Then
            Return
        End If

        storage = value
        OnPropertyChanged(propertyName)
    End Sub

    Private Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
End Class
```

## 4. Add NavHelper.cs

Add this extension class in the **Helpers** folder to the project. This allows the NavigationViewItems to contain a Type property that is used for navigation.
```vbnet
Public Class NavHelper

    Public Shared Function GetNavigateTo(item As NavigationViewItem) As Type
        Return CType(item.GetValue(NavigateToProperty), Type)
    End Function

    Public Shared Sub SetNavigateTo(item As NavigationViewItem, value As Type)
        item.SetValue(NavigateToProperty, value)
    End Sub

    Public Shared ReadOnly NavigateToProperty As DependencyProperty =
        DependencyProperty.RegisterAttached("NavigateTo", GetType(Type), GetType(NavHelper), New PropertyMetadata(Nothing))
End Class
```

## 6. Remove ShellNavigationItem.cs
ShellNavigationItem is no longer used and you should remove it from the project.

## 7. Update XAML code for all pages
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

## 8. Update Navigation View item name for all pages in Resources.resw
As NavigationItems and their names are defined in xaml now, you need to add `.Content` to each of the navigation view item names.
(_for example `Shell_Main` should be changed to `Shell_Main.Content`_)

## 9. Settings Page
If your project contains a SettingsPage you must perform the following steps:
- On **ShellPage.xaml** change **IsSettingsVisible** property to true.
- On **ShellPage.xaml.cs** go to **OnItemInvoked** method and add to the beginning:
```vbnet
If args.IsSettingsInvoked Then
    NavigationService.Navigate(GetType(SettingsPage))
    Return
End If
```

- On **ShellPage.xaml.cs** go to **Frame_Navigated** method and add to the beginning:
```vbnet
If e.SourcePageType = GetType(SettingsPage) Then
    Selected = TryCast(_navigationView.SettingsItem, NavigationViewItem)
    Return
End If
```
