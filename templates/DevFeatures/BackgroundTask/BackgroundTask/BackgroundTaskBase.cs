using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace ItemNamespace.BackgroundTask
{
    public abstract class BackgroundTaskBase
    {
        public bool Match(string name)
        {
            if (name == this.GetType().Name)
            {
                return true; 
            }
            else
            {
                return false;
            }
        }

        public abstract void Register();

        public abstract Task RunAsync(IBackgroundTaskInstance taskInstance);
        
        public abstract void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason);

        public void SubscribeToEvents(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
        }
    }
}