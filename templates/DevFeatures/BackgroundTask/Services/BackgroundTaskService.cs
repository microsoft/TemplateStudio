using Windows.ApplicationModel.Background;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using RootNamespace.Activation;
using Windows.ApplicationModel.Activation;
using RootNamespace.BackgroundTasks;
using RootNamespace.Helper;

namespace ItemNamespace.Services
{
    internal class BackgroundTaskService : ActivationHandler<BackgroundActivatedEventArgs>
    {
        public static IEnumerable<BackgroundTask> BackgroundTasks => backgroundTasks.Value;

        private static readonly Lazy<IEnumerable<BackgroundTask>> backgroundTasks = 
            new Lazy<IEnumerable<BackgroundTask>>(() => CreateInstances());

        public void RegisterBackgroundTasks()
        {
            foreach (var task in BackgroundTasks)
            {
                task.Register();               
            }
        }

        public static BackgroundTaskRegistration GetBackgroundTasksRegistration<T>() where T : BackgroundTask
        {
            if (!BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == typeof(T).Name))
            {
                //TODO: Handle this condition
                return null;
            }
            return (BackgroundTaskRegistration)BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == typeof(T).Name).Value;
        }

        public void Start(IBackgroundTaskInstance taskInstance)
        {
            var task = BackgroundTasks.FirstOrDefault(b => b.Match(taskInstance?.Task?.Name));

            if (task == null)
            {
                //TODO: Handle this condition
                return;
            }

            task.RunAsync(taskInstance).FireAndForget();
        }

        protected override async Task HandleInternalAsync(BackgroundActivatedEventArgs args)
        {
            Start(args.TaskInstance);

            await Task.CompletedTask;
        }

        private static IEnumerable<BackgroundTask> CreateInstances()
        {
            var backgroundTasks = new List<BackgroundTask>();
            return backgroundTasks;
        }
    }
}