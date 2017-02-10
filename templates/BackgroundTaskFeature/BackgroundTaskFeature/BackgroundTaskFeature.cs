using Windows.ApplicationModel.Background;

namespace BackgroundTaskFeature
{
    public sealed class BackgroundTaskFeature : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            //TODO UWPTemplates: Insert the code that should be executed in the background task here. 
            //Remember to use a BackgroundTaskDeferral if you are executing asynchgronous methods.
            //Documentation: https://msdn.microsoft.com/en-us/windows/uwp/launch-resume/support-your-app-with-background-tasks
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
           //TODO UWPTemplates: Insert code to handle the cancelation request here. 
           //Documentation: https://docs.microsoft.com/en-us/windows/uwp/launch-resume/handle-a-cancelled-background-task
        }
    }
}
