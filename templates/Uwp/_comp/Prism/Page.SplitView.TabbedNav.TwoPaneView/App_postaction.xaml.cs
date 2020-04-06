//{[{
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {

        protected override void ConfigureContainer()
        {
//^^
//{[{
            Container.RegisterType<IBackNavigationService, BackNavigationService>();
//}]}
        }
    }
}
