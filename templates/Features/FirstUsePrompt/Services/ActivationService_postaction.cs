using System;

namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task StartupAsync()
        {
            //{[{
            await FirstRunDisplayService.ShowIfAppropriate();
            //}]}
        }
    }
}