using Windows.ApplicationModel.Background;

namespace Param_ItemNamespace.Services
{
    internal interface IBackgroundTaskService
    {
        void RegisterBackgroundTasks();

        void Start(IBackgroundTaskInstance taskInstance);
    }
}
