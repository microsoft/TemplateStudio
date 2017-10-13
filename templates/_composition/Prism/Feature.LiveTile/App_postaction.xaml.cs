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
            Container.RegisterInstance<ILiveTileFeatureService>(new LiveTileFeatureService());
//}]}
        }

        private async Task LaunchApplication(string page, object launchParam)
        {
            NavigationService.Navigate(page, launchParam);            
            Window.Current.Activate();
//{[{
            Container.Resolve<ILiveTileFeatureService>().SampleUpdate();
//}]}
            await Task.CompletedTask;
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
//{[{
            Container.Resolve<ILiveTileService>().EnableQueueAsync();
//}]}
            return base.OnInitializeAsync(args);
        }
    }
}
