
# Multiple views feature
Multiple views feature in Windows Template Studio helps your users to be more productive by letting them view independent parts of your app in separate windows.

In this documentation we are going to explain how to open and close pages on new windows using the Multiple Views feature of Windows Template Studio.

For more information see the multiple view documentation on [docs.microsoft.com](https://docs.microsoft.com/windows/uwp/design/layout/show-multiple-views) or checkout the [official sample](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/MultipleViews).

## Understanding the code
Files added by Multiple views feature:
 - `Services/ViewLifetimeControl.cs`
 - `Services/WindowManagerService.cs`

### WindowManagerService
`WindowManagerService` allows you open pages in a new window using the ApplicationViewSwitcher API. WindowManagerService also includes a method named `IsWindowOpen` that allows you to check if a page is already opened in a new window.

### ViewLifetimeControl
`WindowManagerService` creates an instance of `ViewLifetimeControl` for each page being opened in a new window. This instance allows you to handle the view lifetime event `Released` and gives you access to the window's `Dispatcher` to run code on a safe thread.

## Using WindowManagerService
In this example we are going to show how to open a photo detail in a new window.

### 1. Open the detail page on a new window.
Add a command to open the detail  page in a new window. (Add this Command in `PhotoListViewModel.cs` on **MVVMBasic, MVVMLight, Prism or Caliburn.Micro** framework or in `PhotoListPage.xaml.cs` if your are using **CodeBehind** framework.

```csharp
private ICommand _openPhotoViewCommand;

public ICommand OpenPhotoViewCommand => _openPhotoViewCommand ?? (_openPhotoViewCommand = new RelayCommand(OnOpenView));

private async void OnOpenPhotoView()
{
    await WindowManagerService.Current.TryShowAsStandaloneAsync("PhotoDetail_Title.Text".GetLocalized(), typeof(PhotoDetailPage));
}
```

 ### 2. Handle the DetailPage released event.
 WindowManagerService holds a reference to each window opened. It's important to remove this reference once the window is closed to avoid memory leaks. Suscribe to Release event on the window's ViewLifetimeControl instance to remove this page from `WindowManagerService.Current.SecondaryViews`:

**MVVMBasic, MVVMLight, Prism and Caliburn.Micro**

Override this method on `PhotoDetailPage.xaml.cs`.
```csharp
protected override void OnNavigatedTo(NavigationEventArgs e)
{
    base.OnNavigatedTo(e);
    ViewModel.Initialize(e.Parameter as ViewLifetimeControl);
}
```

Add this code to `PhotoDetailViewModel.cs`.
```csharp
private ViewLifetimeControl _viewLifetimeControl;    

internal void Initialize(ViewLifetimeControl viewLifetimeControl)
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

**CodeBehind**

Add this code to `PhotoDetailViewModel.cs`.
```csharp
public sealed partial class PhotoDetailPage : Page
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