//{**
// This code block adds the BackgroundTaskFeature to the method `CreateInstances()` of the BackgroundTaskService of your project.
//**}
namespace Param_RootNamespace.Services
{
    internal class BackgroundTaskService : IBackgroundTaskService
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
