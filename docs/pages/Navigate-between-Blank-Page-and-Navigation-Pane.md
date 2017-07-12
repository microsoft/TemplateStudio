#Navigate between Blank Page and Navigation Pane

This document will help you create a new way to navigate between Blank Pages and a Navigation Pane. 
This however only implies on a MVMM Light project.

---------------------------------------------------------

After the creating the project, you'll probably notice that the wizard has created a few classes.

We only need to change the following files:
-> Services/ActivationService.cs
-> Services/NavigationService.cs
-> Views/MainPage.xaml.cs (The page with the Frame for 'Navigation Page')
-> ViewModels/MainViewModel.cs (The ViewModel of MainPage)


## Add the NavigationToFrame method 
Firstly, we need to create a method that we will use to Navigate between Blank Pages.
This method will assign the new frame, disables the back button and navigate to the givin page. 

Add this to NavigationService.cs:
```csharp
public bool NavigateToFrame(string pageKey, UIElement frame, object parameter = null, NavigationTransitionInfo infoOverride = null)
{
	lock (_pages)
	{
		if (!_pages.ContainsKey(pageKey))
		{
			throw new ArgumentException($"Page not found: {pageKey}. Did you forget to call NavigationService.Configure?", "pagekey");
		}
		if (frame != null)
		{
			Window.Current.Content = frame;
			Window.Current.Activate();

			Frame.BackStack.Clear();
			Frame.NavigationFailed += (sender, e) =>
			{
				throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
			};
			Frame.Navigated += OnFrameNavigated;
			if (SystemNavigationManager.GetForCurrentView() != null)
			{
				SystemNavigationManager.GetForCurrentView().BackRequested += OnAppViewBackButtonRequested;
			}
		}
		var navigationResult = Frame.Navigate(_pages[pageKey], parameter, infoOverride);
		SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
		return navigationResult;
	}
}
```

You'll notice that there is already a Navigation method present. This method however will only work once the frame is initialized. 
The method above gives you the oppertunity to switch between Frames.

You will see some errors come up. This occurs because we need to move some methods to this class.

Move the following methods from 'ActivationService.cs' to 'NavigationService.cs':
```csharp
private void OnFrameNavigated(object sender, NavigationEventArgs e)
{
	SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = (NavigationService.CanGoBack) ?
		AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
}

private void OnAppViewBackButtonRequested(object sender, BackRequestedEventArgs e)
{
	if (NavigationService.CanGoBack)
	{
		NavigationService.GoBack();
		e.Handled = true;
	}
}
```

Also remove the following code in the 'ActivateAsync' method in 'ActivationService.cs' (this will remove the errors):

```csharp
NavigationService.Frame.NavigationFailed += (sender, e) =>
{
	throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
};
NavigationService.Frame.Navigated += OnFrameNavigated;
if (SystemNavigationManager.GetForCurrentView() != null)
{
	SystemNavigationManager.GetForCurrentView().BackRequested += OnAppViewBackButtonRequested;
}
```

At last, we need to define a 'OnNavigatedTo' method to assign the Frame once you'll navigate to the 'Navigate Pane' Page. (In this case the 'MainPage' class)

Add the following to 'MainPage.xaml.cs':
```csharp
protected override void OnNavigatedTo(NavigationEventArgs e)
{
	base.OnNavigatedTo(e);
	ViewModel.Initialize(MainFrame);
}
```

The `MainFrame` is the name of the Frame that is defined in the 'MainPage.xaml'. For example: `<Frame x:Name="MainFrame"/>`

All done!

To switch between blank pages, you can call the method like: 
	`NavigationService.NavigateToFrame(typeof(LoginPageViewModel).FullName, new Views.LoginPage());`

To go from Blank Page to the page with the Navigation Pane, call the method like this: 
	`NavigationService.NavigateToFrame(typeof(SamplePageViewModel).FullName, new Views.MainPage());`

Once you're in the Navigation Pane, you can call the old method like: 
	`NavigationService.Navigate(typeof(SamplePageViewModel).FullName);`

Don't forget to Register all your pages in the ViewModelLocator!

##Conclusion
We've created a method in 'NavigationService' to navigate between pages without an own defined Frame.
This gives you the possibility to navigate between a Navigation Pane and a Blank Page.
