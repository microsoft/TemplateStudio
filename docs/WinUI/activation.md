# ActivationService & ActivationHandlers

## ActivationService

The ActivationService is in charge of handling the application's initialization and activation.

The method `ActivateAsync()` it the entry point for the application lifecycle event `OnLaunched`.

## ActivationHandlers

The `ActivationService` has a collection of ActivationHandlers, that are registered in the property `IEnumerable<IActivationHandler> _activationHandlers`.

Each class in the application that can handle activation derives from the abstract class `ActivationHandler<T>` (T is the type of ActivationEventArguments the class can handle) and implement the method HandleInternalAsync().
The method `HandleInternalAsync()` is where the actual activation takes place.
The virtual method `CanHandleInternal()` checks if the incoming activation arguments are of the type the ActivationHandler can manage. It can be overwritten to establish further conditions based on the ActivationEventArguments.

### ActivateAsync

**InitializeAsync**

`InitializeAsync` contains services initialization for services that are going to be used as `ActivationHandler`. This method is called before the window is activated. Only code that needs to be executed before app activation should be placed here, as the splash screen is shown while this code is executed.

**StartupAsync**

`StartupAsync` contains initializations of other classes that do not need to happen before app activation and starts processes that will be run after the Window is activated.

### HandleActivationAsync

The `HandleActivationAsync` method gets the first `ActivationHandler` that can handle the arguments of the current activation. It execute the `ActivationHandler<LaunchActivatedEventArgs> _defaultHandler` if no other `ActivationHandler` is selected or the selected `ActivationHandler` does not result in Navigation.

---

## Learn more

- [Using and extending the generated app](./readme.md)
- [Handling navigation within the app](./navigation.md)
- [All docs](../readme.md)
