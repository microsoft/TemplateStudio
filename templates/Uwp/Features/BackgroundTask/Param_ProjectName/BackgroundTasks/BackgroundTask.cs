using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Background;

namespace Param_RootNamespace.BackgroundTasks
{
    public abstract class BackgroundTask
    {
        public abstract void Register();

        public abstract Task RunAsyncInternal(IBackgroundTaskInstance taskInstance);

        public abstract void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason);

        public bool Match(string name)
        {
            return name == GetType().Name;
        }

        public Task RunAsync(IBackgroundTaskInstance taskInstance)
        {
            SubscribeToEvents(taskInstance);

            return RunAsyncInternal(taskInstance);
        }

        public void SubscribeToEvents(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
        }
    }
}
