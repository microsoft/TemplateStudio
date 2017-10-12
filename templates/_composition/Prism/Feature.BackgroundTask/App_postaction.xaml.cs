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
            Container.RegisterInstance<IBackgroundTaskService>(new BackgroundTaskService());
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
            Container.Resolve<IBackgroundTaskService>().Start(args.TaskInstance);
        }
//}]}
        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
//{[{
            Container.Resolve<IBackgroundTaskService>().RegisterBackgroundTasks();
//}]}
            return base.OnInitializeAsync(args);
        }
    }
}
