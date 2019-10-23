//{[{
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
//^^
//{[{
            var themeSelectorService = Container.Resolve<IThemeSelectorService>();
            themeSelectorService.SetTheme();
//}]}
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // App Services
//{[{
            containerRegistry.Register<IThemeSelectorService, ThemeSelectorService>();
//}]}
        }
    }
}