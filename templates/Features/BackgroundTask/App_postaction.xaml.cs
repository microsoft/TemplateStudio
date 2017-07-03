//{**
// This code block adds the OnBackgroundActivated event handler to the App class of your project.
//**}

sealed partial class App : Application
{
    //^^
    //{[{

    protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
    {
        await ActivationService.ActivateAsync(args);
    }
    //}]}
}