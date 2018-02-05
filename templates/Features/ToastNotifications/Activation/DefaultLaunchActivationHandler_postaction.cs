//{**
//This code block adds code to show a sample toast notification on application start to your project.
//**}
//{[{
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            //^^
            //{[{
            // TODO WTS: Remove or change this sample which shows a toast notification when the app is launched.
            // You can use this sample to create toast notifications where needed in your app.
            Singleton<ToastNotificationsFeatureService>.Instance.ShowToastNotificationSample();
            //}]}
            await Task.CompletedTask;
        }
    }
}
