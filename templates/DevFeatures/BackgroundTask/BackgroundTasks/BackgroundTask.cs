using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace ItemNamespace.BackgroundTasks
{
    public abstract class BackgroundTask
    {
        public bool Match(string name)
        {
            if (name == GetType().Name)
            {
                return true; 
            }
            else
            {
                return false;
            }
        }

        public abstract void Register();

       
        public abstract Task RunAsyncInternal(IBackgroundTaskInstance taskInstance);


        public abstract void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason);

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