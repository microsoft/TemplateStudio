//{**
// This code block adds the call to `SampleDataService Initialize` in ActivationService of your project.
//**}
//{[{
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
//{[{
            SampleDataService.Initialize("ms-appx:///Assets");
//}]}
        }
    }
}