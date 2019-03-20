//{[{
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page
    {
//{[{
        private IdentityService IdentityService => Singleton<IdentityService>.Instance;
//}]}

        public ShellPage()
        {
//^^
//{[{
            IdentityService.LoggedOut += OnLoggedOut;
//}]}
        }
//^^
//{[{

        private void OnLoggedOut(object sender, EventArgs e)
        {
            Loaded -= OnLoaded;
            KeyboardAccelerators.Clear();
        }
//}]}
    }
}
