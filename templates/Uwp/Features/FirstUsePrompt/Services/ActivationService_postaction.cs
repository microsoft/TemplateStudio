//{**
// This code block includes code to show the FirstRun control if appropriate on application startup to your project.
//**}

using System;

namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private async Task StartupAsync()
        {
//^^
//{[{
            await FirstRunDisplayService.ShowIfAppropriateAsync();
//}]}
//{??{
            await Task.CompletedTask;
//}??}
        }
    }
}
