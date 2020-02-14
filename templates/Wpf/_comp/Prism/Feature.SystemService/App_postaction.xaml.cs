namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        protected async override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // App Services
//{[{
            containerRegistry.Register<ISystemService, SystemService>();
//}]}
        }
    }
}