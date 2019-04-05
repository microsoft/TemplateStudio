//{[{
using Param_RootNamespace.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.Tests.XUnit
{
    public class Tests
    {
        public void TestSettingsViewModelCreation()
        {
//{[{
            var identityService = new IdentityService();
            var microsoftGraphService = new MicrosoftGraphService();
            var userDataService = new UserDataService(identityService, microsoftGraphService);

            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new SettingsViewModel(identityService, userDataService);
//}]}
        }
    }
}