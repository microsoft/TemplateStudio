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
            Container.RegisterType<IBackgroundTaskService, BackgroundTaskService>(new ContainerControlledLifetimeManager());
//}]}
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            await Task.CompletedTask;
        }

//{[{
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            CreateAndConfigureContainer();
            Container.Resolve<IBackgroundTaskService>().Start(args.TaskInstance);
        }
//}]}
        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
//{[{
            await Container.Resolve<IBackgroundTaskService>().RegisterBackgroundTasksAsync();
//}]}
            await base.OnInitializeAsync(args);
        }
    }
}
