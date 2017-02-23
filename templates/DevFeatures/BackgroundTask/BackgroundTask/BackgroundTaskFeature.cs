using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ItemNamespace.BackgroundTask
{
    public sealed class BackgroundTaskFeature : BackgroundTaskBase
    {
        BackgroundTaskDeferral _deferral = null;

        public override void Register()
        {
            var taskName = this.GetType().Name;
            if (!BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == taskName))
            {
                var builder = new BackgroundTaskBuilder()
                {
                    Name = taskName
                };

                //TODO UWPTemplates: Define your trigger here, 
                //for more info regarding background task registration, see 
                //https://docs.microsoft.com/windows/uwp/launch-resume/create-and-register-an-inproc-background-task
                builder.SetTrigger(new TimeTrigger(15, false));

                //TODO UWPTemplates: Add your conditions here, conditions are optional.
                builder.AddCondition(new SystemCondition(SystemConditionType.UserPresent));
                BackgroundTaskRegistration task = builder.Register();

            }
        }
        public override Task RunAsync(IBackgroundTaskInstance taskInstance)
        {
            //TODO UWPTemplates: Insert the code that should be executed in the background task here. 
            //Remember to use a BackgroundTaskDeferral if you are executing asynchronous methods.
            //Documentation: https://msdn.microsoft.com/windows/uwp/launch-resume/support-your-app-with-background-tasks

            _deferral = taskInstance.GetDeferral();

            return Task.Run(() =>
            {

                //This is an example of sending a toast notification from a background task
                //Documentation: https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-badges-notifications
                SendToastNotification();
            });
        }


        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
           //TODO UWPTemplates: Insert code to handle the cancelation request here. 
           //Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/handle-a-cancelled-background-task
        }

        private void SendToastNotification()
        {
            try
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
                ToastNotificationManager.CreateToastNotifier().Show(toast);

            }
            catch (Exception)
            {
                //TODO: Handle this condition
                throw;
            }
            finally
            {
                _deferral.Complete();
            }
        }
    }
}
