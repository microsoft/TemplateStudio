# App navigation

:heavy_exclamation_mark: There is also a version of [this document with code samples in VB.Net](./navigation.vb.md) :heavy_exclamation_mark: |
-------------------------------------------------------------------------------------------------------------------------------------------- |

## NavigationService

The NavigationService is in charge of handling the navigation between app pages.

NavigationService has different implementations for the different supported design patterns.

- **Code Behind and MVVM Basic**
  - NavigationService is defined as a static class that uses the `Navigate` method to navigate between pages and uses the target page type as a parameter.

- **MVVM Light**
  - The ViewModelLocator creates the NavigationServiceEx instance and registers it with the SimpleIoC container. Each ViewModel and associated page must also be registered as navigation is done by passing the ViewModel name to the `Navigate` method.

```csharp
private NavigationServiceEx _navigationService = new NavigationServiceEx();

public ViewModelLocator()
{
    ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

    Register<HomeViewModel, HomePage>();
    SimpleIoc.Default.Register(() => _navigationService);
}
```

## Navigation initialization

**App.xaml.cs** creates the `ActivationService` and passes it the current App instance, the default navigation target, and, optionally, a UIElement to act as a **navigation shell**. If no shell is specified the current window content will be initialized as a Frame.

Normal launching of the app is passed by the ActivationService to the `DefaultLaunchActivationHandler` and this also sets the default page to display when launching the app.

## Understanding navigation in each project type

Navigation differs between different project types.

- **Blank** project type sets Window.Current.Content as a new Frame and navigates to the HomePage by default. NavigationService will do future navigation in this frame.
- **Navigation Pane** project type sets Window.Current.Content as a new ShellPage instance. This ShellPage will set NavigationService frame to a frame within the page and NavigationService will do future navigation in this frame.
You can find more on configuring code generated with this project type [here](./projectTypes/navigationpane.md).
- **Pivot and Tabs** project type sets Window.Current.Content as a new Frame and navigates to PivotPage that contains a PivotControl, this PivotControl contains one PivotItem for each page. PivotItems contains header text and a Frame set display the configured page. With this project type, the NavigationService does not manage navigating between pivot items, but could be used to navigate away from the PivotPage if necessary.

## Mixed navigation sample

This sample is based on Windows Template Studio 1.3 release and shows an app which includes a _startup page_ that is displayed before navigating to a shell page and then behaving like a Navigation Pane project.
The following code uses [MVVM Basic](../../samples/navigation/MixedNavigationSample.MVVMBasic), versions for [MVVM Light](../../samples/navigation/MixedNavigationSample.MVVMLight) and [Code Behind](../../samples/navigation/MixedNavigationSample.CodeBehind) are also available.

- Step 1. Navigate to the Start Page

In App.xaml.cs the ActivationService has been changed to start on the new page.

```csharp
private ActivationService CreateActivationService()
{
  //This is the default navigation for a NavigationPane project type
  //return new ActivationService(this, typeof(Views.HomePage), new Views.ShellPage());

  //We are going to initialize navigation to a StartPage
  return new ActivationService(this, typeof(Views.StartPage));
}
```

- Step 2. Return to normal **Navigation Pane** navigation.

Navigate to the `ShellPage` and this will reset the NavigationService Frame to it's own custom Frame.
Then navigate to `HomePage` so something is displayed in the shell.
All subsequent navigation just requires a single `Navigate()` call.

```csharp
public class StartViewModel : Observable
{
  public ICommand StartCommand { get; set; }

  public StartViewModel()
  {
    StartCommand = new RelayCommand(OnStart);
  }

  private void OnStart()
  {
    //Navigating to a ShellPage, this will replaces NavigationService frame for an inner frame to change navigation handling.
    NavigationService.Navigate<Views.ShellPage>();

    //Navigating now to a HomePage, this will be the first navigation on a NavigationPane menu
    NavigationService.Navigate<Views.HomePage>();
  }
}
```

The three pages in this sample and the order in which they can be navigated to are shown below.

![Mixed navigation sample](./resources/navigation/MixedNavigationSample.png)

Additional navigation scenarios are covered [here](./navigation-advanced.md).

---

## Learn more

- [Using and extending the generated app](./getting-started-endusers.md)
- [Handling app activation](./activation.md)
- [Adapt the app for specific platforms](./platform-specific-recommendations.md)
- [All docs](../readme.md)
