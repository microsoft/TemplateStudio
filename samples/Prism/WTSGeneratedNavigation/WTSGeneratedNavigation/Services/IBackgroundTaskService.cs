using Windows.ApplicationModel.Background;

namespace WTSGeneratedNavigation.Services
{
    internal interface IBackgroundTaskService
    {
        void RegisterBackgroundTasks();

        void Start(IBackgroundTaskInstance taskInstance);
    }
}
