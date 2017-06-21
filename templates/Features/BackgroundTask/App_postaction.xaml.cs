namespace Param_RootNamespace
{
    public sealed partial class App : Application
    {
//^^
//{[{

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }
 //}]}
    }
}
