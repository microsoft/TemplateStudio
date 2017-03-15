sealed partial class App : Application
{
    //^^
    //{[{

    protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
    {
        await _activationService.ActivateAsync(args);
    }
    //}]}
}