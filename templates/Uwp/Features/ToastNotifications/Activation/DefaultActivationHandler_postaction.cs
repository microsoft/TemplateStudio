//{**
//This code block adds code to show a sample toast notification on application start to your project.
//**}
//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
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
