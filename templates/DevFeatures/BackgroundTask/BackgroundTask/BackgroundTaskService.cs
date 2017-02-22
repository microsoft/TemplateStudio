using Windows.ApplicationModel.Background;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ItemNamespace.BackgroundTask
{
    public static class BackgroundTaskService
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

        public static void RegisterBackgroundTasks()
        {
            foreach (var task in BackgroundTasks)
            {
                task.Register();               
            }
        }

        public static void Start(IBackgroundTaskInstance taskInstance)
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
        public static void FireAndForget(this Task task)
        {

        }
    }
}