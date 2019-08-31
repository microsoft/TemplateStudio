# Update NavigationView to WinUI in MVVMBasic apps

If you have an UWP project created with WinTS with project type **NavigationPane** and framework **Code Behind** please follow these steps to update from NavigationView to Windows UI NavigationView:

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

## 4. Changes in ActivationService.vb

Remove the code to manage back navigation from ActivationService, this code will later be added to the ShellPage.

### VB code you will have to remove:

- `KeyboardAccelerator` shared members.
- `BuildKeyboardAccelerator`, `OnKeyboardAcceleratorInvoked` and `ActivationService_BackRequested` methods.
- `SystemNavigationManager BackRequested` and `NavigationService NavigationFailed` and `Navigated` events handlers registration code inside `ActivateAsync` method.
- Remove unused `imports statements`.

The resulting code should look like this:

(Code in methods: `ActivateFromShareTargetAsync`, `InitializeAsync`, `StartupAsync` and `GetActivationHandlers` might change depending on the pages/features you used. `ActivateFromShareTargetAsync` will appears in ActivationService only if you have added ShareTarger feature.)

```vb
Imports Windows.System
Imports Windows.UI.Core

Imports YourAppName.Activation

Namespace Services
    ' For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.vb.md
    Friend Class ActivationService
        Private ReadOnly _app As App
        Private ReadOnly _shell As Lazy(Of UIElement)
        Private ReadOnly _defaultNavItem As Type

        Public Sub New(app As App, defaultNavItem As Type, Optional shell As Lazy(Of UIElement) = Nothing)
            _app = app
            _shell = shell
            _defaultNavItem = defaultNavItem
        End Sub

        Public Async Function ActivateAsync(activationArgs As Object) As Task
            If IsInteractive(activationArgs) Then
                ' Initialize things like registering background task before the app is loaded
                Await InitializeAsync()

                ' Do not repeat app initialization when the Window already has content,
                ' just ensure that the window is active
                If Window.Current.Content Is Nothing Then
                    ' Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = If(_shell?.Value, New Frame())
                End If
            End If

            Dim activationHandler = GetActivationHandlers().FirstOrDefault(Function(h) h.CanHandle(activationArgs))

            If activationHandler IsNot Nothing Then
                Await activationHandler.HandleAsync(activationArgs)
            End If

            If IsInteractive(activationArgs) Then
                Dim defaultHandler = New DefaultLaunchActivationHandler(_defaultNavItem)
                If defaultHandler.CanHandle(activationArgs) Then
                    Await defaultHandler.HandleAsync(activationArgs)
                End If

                ' Ensure the current window is active
                Window.Current.Activate()

                ' Tasks after activation
                Await StartupAsync()
            End If
        End Function

        Private Async Function InitializeAsync() As Task
            ' Here your Initialize actions
        End Function

        Private Async Function StartupAsync() As Task
            ' Here your Startup actions
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
            ' Here your ActivationHandlers
            Exit Function
        End Function

        Private Function IsInteractive(args As Object) As Boolean
            Return TypeOf args Is IActivatedEventArgs
        End Function
    End Class
End Namespace

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

## 6. Add NavigationViewHeaderBehavior.vb

This behavior allows the NavigationView to hide or customize the NavigationViewHeader depending on the page that is shown, you can read more about this behavior [here](../navigationpane.vb.md). Add the following NavigationViewHeaderBehavior class in Behaviors folder, if your solution doesn't have Behaviors folder you will have to add it.

```vb
Imports Microsoft.Xaml.Interactivity

Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation

Imports WinUI = Microsoft.UI.Xaml.Controls

Imports YourAppName.Services

Namespace Behaviors
    Public Class NavigationViewHeaderBehavior
        Inherits Behavior(Of WinUI.NavigationView)

        Private Shared _current As NavigationViewHeaderBehavior
        Private _currentPage As Page

        Public Property DefaultHeaderTemplate As DataTemplate

        Public Property DefaultHeader As Object
            Get
                Return GetValue(DefaultHeaderProperty)
            End Get
            Set(value As Object)
                SetValue(DefaultHeaderProperty, value)
            End Set
        End Property

        Public Shared ReadOnly DefaultHeaderProperty As DependencyProperty = DependencyProperty.Register("DefaultHeader", GetType(Object), GetType(NavigationViewHeaderBehavior), New PropertyMetadata(Nothing, Sub(d, e) _current.UpdateHeader()))

        Public Shared Function GetHeaderMode(item As Page) As NavigationViewHeaderMode
            Return CType(item.GetValue(HeaderModeProperty), NavigationViewHeaderMode)
        End Function

        Public Shared Sub SetHeaderMode(item As Page, value As NavigationViewHeaderMode)
            item.SetValue(HeaderModeProperty, value)
        End Sub

        Public Shared ReadOnly HeaderModeProperty As DependencyProperty = DependencyProperty.RegisterAttached("HeaderMode", GetType(Boolean), GetType(NavigationViewHeaderBehavior), New PropertyMetadata(NavigationViewHeaderMode.Always, Sub(d, e) _current.UpdateHeader()))

        Public Shared Function GetHeaderContext(item As Page) As Object
            Return item.GetValue(HeaderContextProperty)
        End Function

        Public Shared Sub SetHeaderContext(item As Page, value As Object)
            item.SetValue(HeaderContextProperty, value)
        End Sub

        Public Shared ReadOnly HeaderContextProperty As DependencyProperty = DependencyProperty.RegisterAttached("HeaderContext", GetType(Object), GetType(NavigationViewHeaderBehavior), New PropertyMetadata(Nothing, Sub(d, e) _current.UpdateHeader()))

        Public Shared Function GetHeaderTemplate(item As Page) As DataTemplate
            Return CType(item.GetValue(HeaderTemplateProperty), DataTemplate)
        End Function

        Public Shared Sub SetHeaderTemplate(item As Page, value As DataTemplate)
            item.SetValue(HeaderTemplateProperty, value)
        End Sub

        Public Shared ReadOnly HeaderTemplateProperty As DependencyProperty = DependencyProperty.RegisterAttached("HeaderTemplate", GetType(DataTemplate), GetType(NavigationViewHeaderBehavior), New PropertyMetadata(Nothing, Sub(d, e) _current.UpdateHeaderTemplate()))

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            _current = Me
            AddHandler NavigationService.Navigated, AddressOf OnNavigated
        End Sub

        Private Sub OnNavigated(sender As Object, e As NavigationEventArgs)
            Dim frame = TryCast(sender, Frame)
            Dim page = TryCast(frame.Content, Page)

            If page IsNot Nothing Then
                _currentPage = page
                UpdateHeader()
                UpdateHeaderTemplate()
            End If
        End Sub

        Private Sub UpdateHeader()
            If _currentPage IsNot Nothing Then
                Dim headerMode = GetHeaderMode(_currentPage)

                If headerMode = NavigationViewHeaderMode.Never Then
                    AssociatedObject.Header = Nothing
                    AssociatedObject.AlwaysShowHeader = False
                Else
                    Dim headerFromPage = GetHeaderContext(_currentPage)

                    If headerFromPage IsNot Nothing Then
                        AssociatedObject.Header = headerFromPage
                    Else
                        AssociatedObject.Header = DefaultHeader
                    End If

                    If headerMode = NavigationViewHeaderMode.Always Then
                        AssociatedObject.AlwaysShowHeader = True
                    Else
                        AssociatedObject.AlwaysShowHeader = False
                    End If
                End If
            End If
        End Sub

        Private Sub UpdateHeaderTemplate()
            If _currentPage IsNot Nothing Then
                Dim headerTemplate = GetHeaderTemplate(_currentPage)
                AssociatedObject.HeaderTemplate = If(headerTemplate, DefaultHeaderTemplate)
            End If
        End Sub
    End Class
End Namespace
```

## 7. Add NavigationViewHeaderMode.vb

Add the NavigationViewHeaderBehavior enum in Behaviors folder.

```vb
Namespace Behaviors
    Public Enum NavigationViewHeaderMode
        Always
        Never
        Minimal
    End Enum
End Namespace
```

## 8. Changes in NavHelper.vb

Adjust the Imports statement to move the NavigationViewItem properties to Windows UI NavigationView.

### Add the Imports statement

To

`Microsoft.UI.Xaml.Controls`

## 9. Changes in ShellPage.xaml

The updated ShellPage will contain a WinUI NavigationView that handles back navigation in the app using the NavigationView's BackButton and uses the above mentioned behavior to hide/personalize the NavViewHeader depending on the page shown.

### Xaml code you will have to add (_Implementation below_):

- `i`, `ic`, `winui` and `behaviors` namespaces in page declaration.
- Add `IsBackButtonVisible` and `IsBackEnabled` properties to NavigationView.
- Add `NavigationViewHeaderBehavior` with `DefaultHeader` and `DefaultHeaderTemplate` properties to NavigationView behaviors.
- `Loaded` event.

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
    Loaded="OnLoaded"
    mc:Ignorable="d">

    <winui:NavigationView
        x:Name="navigationView"
        IsBackButtonVisible="Visible"
        IsBackEnabled="{x:Bind IsBackEnabled, Mode=OneWay}"
        SelectedItem="{x:Bind Selected, Mode=OneWay}"
        ItemInvoked="OnItemInvoked"
        IsSettingsVisible="True"
        Background="{ThemeResource SystemControlBackgroundAltHighBrush}">
        <winui:NavigationView.MenuItems>

            <!-- All your pages -->
            <winui:NavigationViewItem x:Uid="Shell_Sample" Icon="Document" helpers:NavHelper.NavigateTo="views:SamplePage" />

        </winui:NavigationView.MenuItems>
        <i:Interaction.Behaviors>
            <behaviors:NavigationViewHeaderBehavior
                DefaultHeader="{x:Bind Selected.Content, Mode=OneWay}">
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

## 10. Changes in ShellPage.xaml.vb

### VB code you will have to add (_Implementation below_):

- Add the following new Imports statements:

```vb
Imports Windows.System
Imports Windows.UI.Xaml.Input
Imports WinUI = Microsoft.UI.Xaml.Controls
```

- Add `WinUI.` namespace alias to `NavigationView`, `NavigationViewItem` and `NavigationViewItemInvokedEventArgs` Data Types.
- Add `_altLeftKeyboardAccelerator`, `_backKeyboardAccelerator` and `IsBackEnabled` members.
- Add `BuildKeyboardAccelerator`, `OnKeyboardAcceleratorInvoked`, `OnLoaded` and `OnBackRequested` methods.
- Subscribe to `BackRequested` event handler in Initialize.
- Set `IsBackEnabled` to `NavigationService.CanGoBack` at the begining of `Frame_Navigated` method.

### VB code you will have to remove

- Remove `HideNavViewBackButton` method.
- Remove from the page constructor `HideNavViewBackButton` call.
- Remove unused Imports statements.

### VB code you will have to update (_Implementation below_):

- Add `_altLeftKeyboardAccelerator` to `KeyboardAccelerators` instead of `ActivationService.AltLeftKeyboardAccelerator`.
- Add `_backKeyboardAccelerator` to `KeyboardAccelerators` instead of `ActivationService.BackKeyboardAccelerator`.

The resulting code should look like this:

```vb
Imports Windows.System
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation

Imports WinUI = Microsoft.UI.Xaml.Controls

Imports YourAppName.Helpers
Imports YourAppName.Services

Namespace Views
    ' TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private ReadOnly _altLeftKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu)
        Private ReadOnly _backKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack)

        Private _isBackEnabled As Boolean
        Private _selected As WinUI.NavigationViewItem

        Public Property IsBackEnabled As Boolean
            Get
                Return _isBackEnabled
            End Get

            Set(value As Boolean)
                [Set](_isBackEnabled, value)
            End Set
        End Property

        Public Property Selected As WinUI.NavigationViewItem
            Get
                Return _selected
            End Get

            Set(value As WinUI.NavigationViewItem)
                [Set](_selected, value)
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            DataContext = Me
            Initialize()
        End Sub

        Private Sub Initialize()
            NavigationService.Frame = shellFrame
            AddHandler NavigationService.Navigated, AddressOf Frame_Navigated
            AddHandler navigationView.BackRequested, AddressOf OnBackRequested
        End Sub

        Private Sub OnLoaded(sender As Object, e As RoutedEventArgs)
            ' Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            ' More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            keyboardAccelerators.Add(_altLeftKeyboardAccelerator)
            keyboardAccelerators.Add(_backKeyboardAccelerator)
        End Sub

        Private Sub OnBackRequested(sender As WinUI.NavigationView, args As WinUI.NavigationViewBackRequestedEventArgs)
            NavigationService.GoBack()
        End Sub

        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            IsBackEnabled = NavigationService.CanGoBack
            If e.SourcePageType = GetType(SettingsPage) Then
                Selected = TryCast(navigationView.SettingsItem, WinUI.NavigationViewItem)
                Return
            End If

            Selected = navigationView.MenuItems.OfType(Of WinUI.NavigationViewItem)().FirstOrDefault(Function(menuItem) IsMenuItemForPageType(menuItem, e.SourcePageType))
        End Sub

        Private Function IsMenuItemForPageType(menuItem As WinUI.NavigationViewItem, sourcePageType As Type) As Boolean
            Dim pageType = TryCast(menuItem.GetValue(NavHelper.NavigateToProperty), Type)
            Return pageType = sourcePageType
        End Function

        Private Sub OnItemInvoked(sender As WinUI.NavigationView, args As WinUI.NavigationViewItemInvokedEventArgs)
            If args.IsSettingsInvoked Then
                NavigationService.Navigate(GetType(SettingsPage))
                Return
            End If

            Dim item = navigationView.MenuItems.OfType(Of WinUI.NavigationViewItem)().First(Function(menuItem) CStr(menuItem.Content) = CStr(args.InvokedItem))
            Dim pageType = TryCast(item.GetValue(NavHelper.NavigateToProperty), Type)
            NavigationService.Navigate(pageType)
        End Sub

        Private Function BuildKeyboardAccelerator(key As VirtualKey, Optional modifiers As VirtualKeyModifiers? = Nothing) As KeyboardAccelerator
            Dim keyboardAccelerator = New KeyboardAccelerator() With {
                .Key = key
            }

            If modifiers.HasValue Then
                keyboardAccelerator.Modifiers = modifiers.Value
            End If

            AddHandler keyboardAccelerator.Invoked, AddressOf OnKeyboardAcceleratorInvoked
            Return keyboardAccelerator
        End Function

        Private Overloads Sub OnKeyboardAcceleratorInvoked(sender As KeyboardAccelerator, args As KeyboardAcceleratorInvokedEventArgs)
            Dim result = NavigationService.GoBack()
            args.Handled = result
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
End Namespace
```
