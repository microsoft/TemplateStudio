using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace ItemNamespace.BackgroundTask
{
    public sealed class BackgroundTaskFeature : BackgroundTaskBase
    {
        public static string Message;
        volatile bool _cancelRequested = false;
        ThreadPoolTimer _periodicTimer = null;
        uint _progress = 0;
        IBackgroundTaskInstance _taskInstance = null;
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
            _deferral = taskInstance.GetDeferral();

            return Task.Run(() =>
            {
                //TODO UWPTemplates: Insert the code that should be executed in the background task here. 
                //Documentation: https://msdn.microsoft.com/windows/uwp/launch-resume/support-your-app-with-background-tasks

                //This example initializes a timer from a background task. The timer counts in steps of 10 until 100 and 
                //updates the values for progress and message in each step. When finished it sends a toast notification.
                //To show the background progress and message on any page in the application, subcribe to the events Progress and Completed exposed by the 
                //backgroundtask registration.  
                //Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/monitor-background-task-progress-and-completion
                _taskInstance = taskInstance;
                _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(PeriodicTimerCallback), TimeSpan.FromSeconds(1));

            });
        }

        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
           //TODO UWPTemplates: Insert code to handle the cancelation request here. 
           //Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/handle-a-cancelled-background-task
        }

        private void PeriodicTimerCallback(ThreadPoolTimer timer)
        {
            if ((_cancelRequested == false) && (_progress < 100))
            {
                _progress += 10;
                _taskInstance.Progress = _progress;
                Message = $"Background Task { _taskInstance.Task.Name} running";
            }
            else
            {
                _periodicTimer.Cancel();

                if (_cancelRequested)
                {
                    Message = $"Background Task {_taskInstance.Task.Name} cancelled";
                }
                else
                {
                    Message = $"Background Task {_taskInstance.Task.Name} finished";
                }
                SendToastNotification();
                _deferral.Complete();
            }
        }

        private void SendToastNotification()
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
    }
}
