namespace RootNamespace
{
    sealed partial class App : Application
    {
        public App()
        {
            //^^
            Services.LiveTileService.EnableQueue();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            //^^
            Services.LiveTileService.SampleUpdate();
        }
    }
}