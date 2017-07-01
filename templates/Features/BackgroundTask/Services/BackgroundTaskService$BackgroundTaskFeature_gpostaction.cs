namespace Param_ItemNamespace.Services
{
    internal class BackgroundTaskService : ActivationHandler<BackgroundActivatedEventArgs>
    {
        private static IEnumerable<BackgroundTask> CreateInstances()
        {
            var backgroundTasks = new List<BackgroundTask>();
//^^
//{[{
            backgroundTasks.Add(new BackgroundTaskFeature());
//}]}
            return backgroundTasks;
        }
    }
}