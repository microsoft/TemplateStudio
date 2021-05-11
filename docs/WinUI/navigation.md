# App navigation

## NavigationService

The `NavigationService` is in charge of handling the navigation between app pages.

`NavigationService` is a class consumed using IoC that uses the `NavigateTo` method to navigate between pages and uses the page key (registered ViewModel full name) as a parameter.

## Navigation initialization

The `ActivateAsync` method in the `ActivationService` class tries to get a `ShellPage` from the `IoC`, if the shell is not registered in the `ConfigureServices` method from the `App` file the `App.MainWindow` content will be initialized as a new Frame.

Classes that extend from `ActivationHandler` can realize the first navigation in the App, if it doesn't happen, the `DefaultActivationHandler` navigates to the home page.

---

## Learn more

- [Using and extending the generated app](./readme.md)
- [Handling app activation](./activation.md)
- [All docs](../readme.md)
