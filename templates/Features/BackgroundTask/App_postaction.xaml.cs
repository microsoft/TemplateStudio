sealed partial class App
{
    //^^
    //{[{

    protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
    {
        await ActivationService.ActivateAsync(args);
    }
    //}]}
}