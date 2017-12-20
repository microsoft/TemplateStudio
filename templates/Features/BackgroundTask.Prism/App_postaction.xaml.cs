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

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            return Task.CompletedTask;
        }

//{[{
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            if (Container == null)
            {
                CreateAndConfigureContainer();
                ConfigureContainer();
            }

            Container.Resolve<IBackgroundTaskService>().Start(args.TaskInstance);
        }
//}]}
        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
//{[{
            Container.Resolve<IBackgroundTaskService>().RegisterBackgroundTasks();
//}]}
            await base.OnInitializeAsync(args);
        }
    }
}
