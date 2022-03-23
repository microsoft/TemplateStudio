//{**
//**}

using System;

namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private async Task StartupAsync()
        {
//{[{

            // TODO WTS: This is a sample to demonstrate how to add a UserActivity. Please adapt and move this method call to where you consider convenient in your app.
            await UserActivityService.AddSampleUserActivity();
//}]}
//{??{
            await Task.CompletedTask;
//}??}
        }
    }
}
