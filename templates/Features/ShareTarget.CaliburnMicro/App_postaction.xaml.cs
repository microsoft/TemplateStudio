//{**
// This code block adds the OnShareTargetActivated event handler to the App class of your project.
//**}
namespace Param_RootNamespace
{
    public sealed partial class App
    {
//^^
//{[{

        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            await ActivationService.ActivateFromShareTargetAsync(args);
        }
//}]}
    }
}
