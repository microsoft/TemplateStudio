sealed partial public class App : Application
{
    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected async override void OnLaunched(LaunchActivatedEventArgs e)
    {
        //TODO: Other elements of this method
        RegisterBackgroundTask();
    }

    private void RegisterBackgroundTask()
    {
        string taskEntryPoint = "BasicBackgroundTaskFeature.BasicBackgroundTaskFeatureBackgroundTask";
        string taskName = "BasicBackgroundTaskFeatureBackgroundTask";
        var trigger = new TimeTrigger(15, false);

        if (!BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == taskName))
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = taskName;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);
            BackgroundTaskRegistration task = builder.Register();
        }
    }
}