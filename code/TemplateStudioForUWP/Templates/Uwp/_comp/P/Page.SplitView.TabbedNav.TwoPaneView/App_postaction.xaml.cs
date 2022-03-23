//{[{
using Param_RootNamespace.Services;
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
