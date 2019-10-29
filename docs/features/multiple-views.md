# Multiple views

Multiple views feature in Windows Template Studio helps your users to be more productive by letting them view independent parts of your app in separate windows.

In this documentation we are going to explain how to open and close pages on new windows using the Multiple Views feature of Windows Template Studio.

For more information see the multiple view documentation on [docs.microsoft.com](https://docs.microsoft.com/windows/uwp/design/layout/show-multiple-views) or checkout the [official sample](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/MultipleViews).

## Understanding the code

Files added by Multiple views feature:

- `Services/WindowManagerService.cs`
- `Services/ViewLifetimeControl.cs`

### WindowManagerService

`WindowManagerService` allows you open pages in a new window using the ApplicationViewSwitcher API. WindowManagerService also includes a method named `IsWindowOpen` that allows you to check if a page is already open in another window.

### ViewLifetimeControl

`WindowManagerService` creates an instance of `ViewLifetimeControl` for each page being opened in a new window. This instance allows you to handle the view lifetime event `Released` and gives you access to the window's `Dispatcher` to run code on a safe thread.

## Using WindowManagerService

In this example we are going to show how to open a photo detail in a new window.

## 1. Create a new app with two pages

Create a new application using Windows Template Studio and add another blank page named `Secondary`. The idea is to create a button in the `Main` page to open the `Secondary` page in a new window.

## 2. Open the secondary page on a new window.

Open the secondary page in a new window using a Button in MainPage.

**`CodeBehind`**

Create a button in MainPage.xaml

```xml
<Button Content="Open secondary page" Click="OpenSecondaryPageButton_Click" />
```

Create a event handler in MainPage.xaml.cs

```csharp
private async void OpenSecondaryPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
{
    await WindowManagerService.Current.TryShowAsStandaloneAsync("Secondary_Title.Text".GetLocalized(), typeof(SecondaryPage));
}
```

**`MVVMBasic, MVVMLight`**

Create a button in MainPage.xaml

```xml
<Button Content="Open secondary page" Command="{x:Bind ViewModel.OpenSecondaryPageCommand}" />
```

Create a command in MainViewModel.cs

```csharp
private ICommand _openSecondaryPageCommand;

public ICommand OpenSecondaryPageCommand => _openSecondaryPageCommand ?? (_openSecondaryPageCommand = new RelayCommand(OnOpenSecondaryPage));

private async void OnOpenSecondaryPage()
{
    await WindowManagerService.Current.TryShowAsStandaloneAsync("Secondary_Title.Text".GetLocalized(), typeof(SecondaryPage));
}
```

**`Prism`**

Create a button in MainPage.xaml

```xml
<Button Content="Open secondary page" Command="{x:Bind ViewModel.OpenSecondaryPageCommand}" />
```

Create a command in MainViewModel.cs

```csharp
private ICommand _openSecondaryPageCommand;

public ICommand OpenSecondaryPageCommand => _openSecondaryPageCommand ?? (_openSecondaryPageCommand = new DelegateCommand(OnOpenSecondaryPage));

private async void OnOpenSecondaryPage()
{
    await WindowManagerService.Current.TryShowAsStandaloneAsync("Secondary_Title.Text".GetLocalized(), typeof(SecondaryPage));
}
```

**`Caliburn.Micro`**

Create a button in MainPage.xaml

```xml
<Button Content="Open secondary page" cm:Message.Attach="OpenSecondaryPage" />
```

Create a command in MainViewModel.cs

```csharp
public async void OpenSecondaryPage()
{
    await WindowManagerService.Current.TryShowAsStandaloneAsync("Secondary_Title.Text".GetLocalized(), typeof(SecondaryPage));
}
```

## 3. Handle the SecondaryPage released event.

 WindowManagerService holds a reference to each window opened. It's important to remove this reference once the window is closed to avoid memory leaks. Subscribe to the Release event on the window's ViewLifetimeControl instance to remove this page from `WindowManagerService.Current.SecondaryViews`:

**`MVVMBasic, MVVMLight, Prism and Caliburn.Micro`**

Override this method on `SecondaryPage.xaml.cs`.

```csharp
protected override void OnNavigatedTo(NavigationEventArgs e)
{
    base.OnNavigatedTo(e);
    ViewModel.Initialize(e.Parameter as ViewLifetimeControl);
}
```

Add this code to `SecondaryViewModel.cs`.

```csharp
private ViewLifetimeControl _viewLifetimeControl;

public void Initialize(ViewLifetimeControl viewLifetimeControl)
{
    _viewLifetimeControl = viewLifetimeControl;
    _viewLifetimeControl.Released += OnViewLifetimeControlReleased;
}

private async void OnViewLifetimeControlReleased(object sender, EventArgs e)
{
    _viewLifetimeControl.Released -= OnViewLifetimeControlReleased;
    await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
    {
        WindowManagerService.Current.SecondaryViews.Remove(_viewLifetimeControl);
    });
}
```

**`CodeBehind`**

Add this code to `SecondaryViewModel.cs`.

```csharp
public sealed partial class SecondaryPage : Page
{
    private ViewLifetimeControl _viewLifetimeControl;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        _viewLifetimeControl = e.Parameter as ViewLifetimeControl;
        _viewLifetimeControl.Released += OnViewLifetimeControlReleased;
    }

    private async void OnViewLifetimeControlReleased(object sender, EventArgs e)
    {
        _viewLifetimeControl.Released -= OnViewLifetimeControlReleased;
        await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        {
            WindowManagerService.Current.SecondaryViews.Remove(_viewLifetimeControl);
        });
    }
}
```

## Adapt pages to show in a new window

When showing a page in a new window, you must adapt your page to the new context:

- Do not use NavigationService: NavigationService only manages the main Frame, so if you need to navigate in the new window you must use the native methods of the new frame control configured in the new window.
- Review and move code from OnNavigatedFrom event to OnViewLifetimeControlReleased to ensure page resources are cleaned up correctly.
- If you need to make changes to the UI thread, you have to encapsulate those calls within _viewLifetimeControl.Dispatcher.
- Do not use NavigationViewHeaderBehavior: The NavigationViewHeaderBehavior is used to configure the page headers within the main NavigationView. This component does not exist in the new window.
