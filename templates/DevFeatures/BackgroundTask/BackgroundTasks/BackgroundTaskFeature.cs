using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Background;
using Windows.System.Threading;

namespace ItemNamespace.BackgroundTasks
{
    public sealed class BackgroundTaskFeature : BackgroundTask
    {
        public static string Message;

        private volatile bool _cancelRequested = false;
        private IBackgroundTaskInstance _taskInstance;
        private BackgroundTaskDeferral _deferral;

        public override void Register()
        {
            var taskName = GetType().Name;
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
                builder.Register();
            }
        }

        public override Task RunAsyncInternal(IBackgroundTaskInstance taskInstance)
        {
            if (taskInstance == null)
            {
                return null;
            }

            _deferral = taskInstance.GetDeferral();

            return Task.Run(() =>
            {
                //TODO UWPTemplates: Insert the code that should be executed in the background task here. 
                //Documentation: https://docs.microsoft.com/en-us/windows/uwp/launch-resume/support-your-app-with-background-tasks

                //This sample initializes a timer, the timer counts in steps of 10 until 100 and updates the values for progress and message in each step. 
                //To show the background progress and message on any page in the application, subcribe to the events 
                //Progress and Completed exposed by the backgroundtask registration.  
                //Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/monitor-background-task-progress-and-completion
                _taskInstance = taskInstance;
                ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(SampleTimerCallback), TimeSpan.FromSeconds(1));
            });
        }

        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
           //TODO UWPTemplates: Insert code to handle the cancelation request here. 
           //Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/handle-a-cancelled-background-task
        }

        private void SampleTimerCallback(ThreadPoolTimer timer)
        {
            if ((_cancelRequested == false) && (_taskInstance.Progress < 100))
            {
                _taskInstance.Progress += 10;
                Message = $"Background Task { _taskInstance.Task.Name} running";
            }
            else
            {
                timer.Cancel();

                if (_cancelRequested)
                {
                    Message = $"Background Task {_taskInstance.Task.Name} cancelled";
                }
                else
                {
                    Message = $"Background Task {_taskInstance.Task.Name} finished";
                }
              
                _deferral?.Complete();
            }
        }
    }
}
