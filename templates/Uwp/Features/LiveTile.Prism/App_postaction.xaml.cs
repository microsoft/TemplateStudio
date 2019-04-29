using Windows.UI.Xaml;
//^^
//{[{
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
//{[{
            Container.RegisterType<ILiveTileFeatureService, LiveTileFeatureService>(new ContainerControlledLifetimeManager());
//}]}
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            Window.Current.Activate();
//{[{
            Container.Resolve<ILiveTileFeatureService>().SampleUpdate();
//}]}
            await Task.CompletedTask;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);
//^^
//{[{
            await Container.Resolve<ILiveTileFeatureService>().EnableQueueAsync().ConfigureAwait(false);
//}]}
        }
    }
}
