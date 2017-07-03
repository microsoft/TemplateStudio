//{**
//This code block adds code to show a sample toast notification on application start to your project.
//**}

//{[{
using Param_RootNamespace.Helpers;
//}]}

namespace Param_RootNamespace.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            //^^
            //{[{
            // TODO UWPTemplates: This is a sample on how to show a toast notification.
            // You can use this sample to create toast notifications where needed in your app.
            Singleton<ToastNotificationsFeatureService>.Instance.ShowToastNotificationSample();
            //}]}
            await Task.CompletedTask;
        }

    }
}
