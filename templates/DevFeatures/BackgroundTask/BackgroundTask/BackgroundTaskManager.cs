using Windows.ApplicationModel.Background;

namespace ItemNamespace.BackgroundTask
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