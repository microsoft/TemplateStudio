# App navigation

## NavigationService

The `NavigationService` is in charge of handling the navigation between app pages.

`NavigationService` is a class consumed using IoC that uses the `NavigateTo` method to navigate between pages. It uses the page key as a parameter. These page keys are registered in the `PageService` constructor and correspond to the Page ViewModel `FullName`.

All pages ViewModels can extend from `INavigationAware` interface to run code on navigation events.

```csharp
public class MainViewModel : ObservableRecipient, INavigationAware
{
    public void OnNavigatedTo(object parameter)
    {
        // Run code when the app navigates to this page
    }

    public void OnNavigatedFrom()
    {
        // Run code when the app navigates away from this page
    }
}
```

## Navigation initialization

The `ActivateAsync` method in the `ActivationService` class tries to get a `ShellPage` from the `IoC`, if the shell is not registered in the `ConfigureServices` method from the `App` file the `App.MainWindow` content will be initialized as a new Frame.

Classes that extend from `ActivationHandler` can perform the first navigation in the App, if it doesn't happen, the `DefaultActivationHandler` navigates to the home page.

## Understanding navigation in each project type

### Navigation Pane

This project type adds a ShellPage that sets NavigationService frame to a frame within the NavigationView. The NavigationService will do future navigation in this frame.

```xml
<Page>
    <NavigationView>
        <Grid>
            <Frame x:Name="shellFrame" />
        </Grid>
    </NavigationView>
</Page>
```

---

## Learn more

- [Using and extending the generated app](./readme.md)
- [Handling app activation](./activation.md)
- [All docs](../readme.md)
