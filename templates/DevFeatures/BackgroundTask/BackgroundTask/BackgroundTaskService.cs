using Windows.ApplicationModel.Background;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using RootNamespace.Activation;
using Windows.ApplicationModel.Activation;

namespace ItemNamespace.BackgroundTask
{
    class BackgroundTaskService : ActivationHandler<BackgroundActivatedEventArgs>
    {
        private static readonly Lazy<IEnumerable<BackgroundTaskBase>> backgroundTasks = 
            new Lazy<IEnumerable<BackgroundTaskBase>>(() => CreateInstances());

        public static IEnumerable<BackgroundTaskBase> BackgroundTasks => backgroundTasks.Value;

        private static IEnumerable<BackgroundTaskBase> CreateInstances()
        {
            var backgroundTasks = new List<BackgroundTaskBase>();
            //BACKGROUNDTASK_ANCHOR

            return backgroundTasks;
        }

        public void RegisterBackgroundTasks()
        {
            foreach (var task in BackgroundTasks)
            {
                task.Register();               
            }
        }

        public BackgroundTaskRegistration GetBackgroundTasksRegistration(Type task)
        {
            return (BackgroundTaskRegistration)BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == task.Name).Value;
        }

        public void Start(IBackgroundTaskInstance taskInstance)
        {
            var task = BackgroundTasks.FirstOrDefault(b => b.Match(taskInstance.Task.Name));

            if (task == null)
            {
                //TODO: Handle this condition
                return;
            }
            task.SubscribeToEvents(taskInstance);
            task.RunAsync(taskInstance).FireAndForget();
          
        }

        protected override async Task HandleInternalAsync(BackgroundActivatedEventArgs args)
        {
            Start(args.TaskInstance);

            await Task.FromResult(true).ConfigureAwait(false);
        }
    }

    public static class TaskExtensions
    {
        public static void FireAndForget(this Task task)
        {

        }
    }
}