using BackgroundTask;
using Windows.ApplicationModel.Background;

//TODO: Review namespace
namespace BackgroundTaskFeature
{
    public static class BackgroundTaskManager
    {
        public static void Start(IBackgroundTaskInstance taskInstance)
        {
            switch (taskInstance.Task.Name)
            {
                case "BackgroundTaskFeature":
                    var task = new BackgroundTaskFeature();
                    task.Run(taskInstance);
                    break;
                default:
                    break;

            }
        }
    }
}