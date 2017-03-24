using System;
using System.Reflection;
using System.Threading.Tasks;
using RootNamespace.Extensions;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;

using RootNamespace.Activation;

namespace ItemNamespace.Services
{
    class SuspendAndResumeService : ActivationHandler<LaunchActivatedEventArgs>
    {
        //TODO UWPTEMPLATES: For more information regarding the application lifecycle and how to handle suspend and resume, please see: 
        //https://docs.microsoft.com/windows/uwp/launch-resume/app-lifecycle

        private const string stateFilename = "suspensionState";

        //TODO UWPTEMPLATES: This event is fired just before the app enters in background. Subscribe to this event if you want to save your current state.
        public event EventHandler<OnBackgroundEnteringEventArgs> OnBackgroundEntering;

        public async Task SaveStateAsync()
        {
            var suspensionState = new SuspensionState()
            {
                SuspensionDate = DateTime.Now
            };

            var target = OnBackgroundEntering?.Target.GetType();
            var onBackgroundEnteringArgs = new OnBackgroundEnteringEventArgs(suspensionState, target);

            OnBackgroundEntering?.Invoke(this, onBackgroundEnteringArgs);
            await ApplicationData.Current.LocalFolder.SaveAsync(stateFilename, onBackgroundEnteringArgs);
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            await RestoreStateAsync();
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            return args.PreviousExecutionState == ApplicationExecutionState.Terminated;
        }

        private async Task RestoreStateAsync()
        {
            var saveState = await ApplicationData.Current.LocalFolder.ReadAsync<OnBackgroundEnteringEventArgs>(stateFilename);
           
        }
    }
}
