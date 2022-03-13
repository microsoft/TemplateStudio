using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;

using Param_RootNamespace.Activation;
using Param_RootNamespace.BackgroundTasks;
using Param_RootNamespace.Core.Helpers;

namespace Param_RootNamespace.Services
{
    internal class BackgroundTaskService : ActivationHandler<BackgroundActivatedEventArgs>
    {
        public static IEnumerable<BackgroundTask> BackgroundTasks => BackgroundTaskInstances.Value;

        private static readonly Lazy<IEnumerable<BackgroundTask>> BackgroundTaskInstances =
            new Lazy<IEnumerable<BackgroundTask>>(() => CreateInstances());

        public async Task RegisterBackgroundTasksAsync()
        {
            BackgroundExecutionManager.RemoveAccess();
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result == BackgroundAccessStatus.DeniedBySystemPolicy
                || result == BackgroundAccessStatus.DeniedByUser)
            {
                return;
            }

            foreach (var task in BackgroundTasks)
            {
                task.Register();
            }
        }

        public static BackgroundTaskRegistration GetBackgroundTasksRegistration<T>()
            where T : BackgroundTask
        {
            if (!BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == typeof(T).Name))
            {
                // This condition should not be met. If it is it means the background task was not registered correctly.
                // Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
                return null;
            }

            return (BackgroundTaskRegistration)BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == typeof(T).Name).Value;
        }

        public void Start(IBackgroundTaskInstance taskInstance)
        {
            var task = BackgroundTasks.FirstOrDefault(b => b.Match(taskInstance?.Task?.Name));

            if (task == null)
            {
                // This condition should not be met. It is it it means the background task to start was not found in the background tasks managed by this service.
                // Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
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
