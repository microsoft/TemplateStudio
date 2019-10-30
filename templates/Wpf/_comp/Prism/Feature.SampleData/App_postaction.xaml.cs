namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // App Services
//{[{
            containerRegistry.Register<ISampleDataService, SampleDataService>();
//}]}
        }
    }
}