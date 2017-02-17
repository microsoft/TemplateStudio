using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ItemNamespace.BackgroundTask
{
    public sealed class BackgroundTaskFeature 
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            //TODO UWPTemplates: Insert the code that should be executed in the background task here. 
            //Remember to use a BackgroundTaskDeferral if you are executing asynchgronous methods.
            //Documentation: https://msdn.microsoft.com/windows/uwp/launch-resume/support-your-app-with-background-tasks

            //This is an example of sending a toast notification from a background task
            //Documentation: https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-badges-notifications
            var toast = ConstructToastNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
           //TODO UWPTemplates: Insert code to handle the cancelation request here. 
           //Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/handle-a-cancelled-background-task
        }

        private ToastNotification ConstructToastNotification()
        {
            string xml = 
            $@"<toast activationType='foreground'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>Action - text</text>
                            <text>Make sure left button on the toast has the text ""ok"" on it, and the right button has the text ""cancel"" on it.</text>
                        </binding>
                    </visual>
                    <actions>
                        <action
                            content='ok'
                            activationType='foreground'
                            arguments='ok'/>
                        <action
                            content='cancel'
                            activationType='foreground'
                            arguments='cancel'/>
                    </actions>
                </toast>";

            // Parse to XML
            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(xml);

            // Generate toast
            var toast = new ToastNotification(toastXml);

            return toast;
        }
    }
}
