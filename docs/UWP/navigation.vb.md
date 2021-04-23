# App navigation

:heavy_exclamation_mark: There is also a version of [this document with code samples in C#](./navigation.md) :heavy_exclamation_mark: |
------------------------------------------------------------------------------------------------------------------------------------- |

## NavigationService

The `NavigationService` is in charge of handling the navigation between app pages.

`NavigationService` is defined as a static class that uses the `Navigate` method to navigate between pages and uses the target page type as a parameter.

## Navigation initialization

**App.xaml.vb** creates the `ActivationService` and passes it the current App instance, the default navigation target, and, optionally, a UIElement to act as a **navigation shell**. If no shell is specified the current window content will be initialized as a Frame.

Normal launching of the app is passed by the `ActivationService` to the `DefaultLaunchActivationHandler` and this also sets the default page to display when launching the app.

## Understanding navigation in each project type

Navigation differs between different project types.

- **Blank** project type sets Window.Current.Content as a new Frame and navigates to the HomePage by default. NavigationService will do future navigation in this frame.
- **Navigation Pane** project type sets Window.Current.Content as a new ShellPage instance. This ShellPage will set NavigationService frame to a frame within the page and NavigationService will do future navigation in this frame.
You can find more on configuring code generated with this project type [here](./projectTypes/navigationpane.md).
- **Pivot and Tabs** project type sets Window.Current.Content as a new Frame and navigates to PivotPage that contains a PivotControl, this PivotControl contains one PivotItem for each page. PivotItems contains header text and a Frame set display the configured page. With this project type, the NavigationService does not manage navigating between pivot items, but could be used to navigate away from the PivotPage if necessary.

## Mixed navigation sample

Sample projects that show mixed navigation are available from [the C# version of this document](./navigation.md)

![Mixed navigation sample](./resources/navigation/MixedNavigationSample.png)

---

## Learn more

- [Using and extending the generated app](./getting-started-endusers.md)
- [Handling app activation](./activation.md)
- [Adapt the app for specific platforms](./platform-specific-recommendations.md)
- [All docs](../readme.md)
