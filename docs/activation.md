# ActivationService & ActivationHandlers

## ActivationService
The ActivationService is in charge of handling the applications initialization and activation. 
 
With the method ActivateAsync() it has one common entry point that is called from the app lifecycle events OnLaunched, OnActivated and OnBackgroundActivated. (For more information on application lifecycle and it events see
[Windows 10 universal Windows platform (UWP) app lifecycle](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/app-lifecycle). 
 
## ActivationHandlers
For choosing the concrete type of activation the ActivationService relies on ActivationHandlers, that are registered in the method GetActivationHandlers(). 
 
Each class in the application that can handle application activation should derive from the abstract class ActivationHandler<T> (T is the type of ActivationEventArguments the class can handle) and implement the method HandleInternalAsync(). 
The method HandleInternalAsync() is where the actual activation takes place. 
The virtual method CanHandleInternal() checks if the incoming activation arguments are of the type the ActivationHandler can manage. It can be overwritten to establish further conditions based on the ActivationEventArguments.
 
### ActivationHandlers sample
We'll have look at the SuspendAndResumeService to see how activation works in detail: 
 
```csharp
protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
{
    return args.PreviousExecutionState == ApplicationExecutionState.Terminated;
}

protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
{
    await RestoreStateAsync();
}

private async Task RestoreStateAsync()
{
    var saveState = await ApplicationData.Current.LocalFolder.ReadAsync<OnBackgroundEnteringEventArgs>(stateFilename);
    if (typeof(Page).IsAssignableFrom(saveState?.Target))
    {
        NavigationService.Navigate(saveState.Target, saveState.SuspensionState);
    }
}
```
The CanHandleInternal() method was overwritten here, to handle activation only in case the previous executionState is Terminated. 
The HandleInternalAsync() method restores the previously stored application state and navigates to the stored page.

## What else happens on activation?

When executing ActivatedAsync(), the ActivationService retrieves the first ActivationHandler able to handle the current activation (evaluating CanHandleInternal() of all registered ActivationHandlers) and invokes it. 
 
In case of interactive activation (for example Launch or Activation from LiveTile) the ActivationService additionally executes the following steps: 
* Initialize app (InitializeAsync()) (f.ex register background tasks, set app theme)
* If there is no current content a frame is created and navigation events handlers are added
* If no ActivationHandler is found, a DefaultLaunchActivationHandler is instantiated, that is in charge of navigating to the default page. 
* Activates the current windows
* Executes StartUp Actions (StartupAsync())
 
You can use InitializeAsync and StartupAsync to add code that should be executed on application initialization and startup.


## How to add activation from File Association?

Let's add activation from a file association

