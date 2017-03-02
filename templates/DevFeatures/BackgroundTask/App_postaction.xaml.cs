using ItemNamespace.BackgroundTask;

namespace RootNamespace
{
    sealed partial class App : Application
    {
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            //^^
            BackgroundTaskService.RegisterBackgroundTasks();
        }
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args) 
        {
            //^^
            BackgroundTaskService.Start(args.TaskInstance);
        }
    }
}