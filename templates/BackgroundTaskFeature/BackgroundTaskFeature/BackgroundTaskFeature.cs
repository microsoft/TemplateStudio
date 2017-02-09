using Windows.ApplicationModel.Background;

namespace BackgroundTaskFeature
{
    public sealed class BackgroundTaskFeature : IBackgroundTask
    {
        volatile bool _cancelRequested = false;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            //TODO UWPTemplates: Insert the code that should be executed in the background task here. 
            //Remember to use a BackgroundTaskDeferral if you are executing asynchgronous methods.

        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _cancelRequested = true;
        }
    }
}
