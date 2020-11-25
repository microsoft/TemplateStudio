**Notice: MVVM Basic has been superseded by the MVVM Toolkit and will be removed as an option in a future version of Windows Template Studio.**

This is a generic version of a MVVM pattern.  The [Model-View-ViewModel pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) can be used on all XAML platforms. Its intent is to provide a clean separation of concerns between the user interface controls and their logic.

There are three core components in the MVVM pattern: the model, the view, and the view model. Each serves a distinct and separate role.

MVVM Basic is not a framework but provides the minimum functionality to create an app using the Model-View-ViewModel (MVVM) pattern.
Use it if you can't or don't want to use a 3rd party MVVM Framework.

MVVM Basic is not intended to be a fully features MVVM Framework and does not include some features that other frameworks do. ViewModel-first navigation, IOC, and messaging being the most obvious ones. If you want these features then choose a framework that supports them.

Projects created with MVVM Basic contain two important classes, `Observable` and `RelayCommand`.
**Observable** contains an implementation of the `INotifyPropertyChanged` interface and is used as a base class for all view models. This makes it easy to update bound properties on the View.
**RelayCommand** contains an implementation of the `ICommand` interface to make it easy to have the View call commands on the ViewModel, rather than handle UI events directly.
