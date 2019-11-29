using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

using Param_RootNamespace.Activation;
using Param_RootNamespace.Helpers;

namespace Param_RootNamespace.Services
{
    // The SuspendAndResumeService allows you to save the App data before the App is being suspended (or enters in background state).
    // In case the App is terminated during suspension, the data is restored during App launch by this ActivationHandler.
    // In case the App is resumed without being terminated no data should be lost, a resume event is fired that allows you to refresh App data that might
    // be outdated (e.g data from online feeds)
    // Documentation:
    //     * How to implement and test: https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/features/suspend-and-resume.md
    //     * Application Lifecycle: https://docs.microsoft.com/windows/uwp/launch-resume/app-lifecycle
    internal class SuspendAndResumeService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private const string StateFilename = "SuspendAndResumeState";

        // This method saves the application state before entering background state. It fires the event OnBackgroundEntering to collect
        // state data from the current subscriber and saves it to the local storage.
        public async Task<bool> SaveStateAsync()
        {
            if (OnBackgroundEntering == null)
            {
                return false;
            }

            try
            {
                var suspensionState = new SuspensionState()
                {
                    SuspensionDate = DateTime.Now
                };

                var target = OnBackgroundEntering?.Target.GetType();
                var onBackgroundEnteringArgs = new SuspendAndResumeArgs(suspensionState, target);

                OnBackgroundEntering?.Invoke(this, onBackgroundEnteringArgs);

                await ApplicationData.Current.LocalFolder.SaveAsync(StateFilename, onBackgroundEnteringArgs);
                return true;
            }
            catch (Exception)
            {
                // TODO WTS: Save state can fail in rare conditions, please handle exceptions as appropriate to your scenario.
                return false;
            }
        }

        // This method allows subscribers to refresh data that might be outdated when the App is resuming from suspension.
        // If the App was terminated during suspension this event will not fire, data restore is handled by the method HandleInternalAsync.
        public void ResumeApp()
        {
            OnResuming?.Invoke(this, EventArgs.Empty);
        }

        public async Task RestoreSuspendAndResumeData()
        {
            var saveState = await GetSuspendAndResumeData();
            if (saveState != null)
            {
                OnDataRestored?.Invoke(this, saveState);
            }
        }

        // This method restores application state when the App is launched after termination, it navigates to the stored Page passing the recovered state data.
        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            var saveState = await GetSuspendAndResumeData();
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            // Application State must only be restored if the App was terminated during suspension.
            return args.PreviousExecutionState == ApplicationExecutionState.Terminated;
        }

        public async Task<SuspendAndResumeArgs> GetSuspendAndResumeData()
        {
            var saveState = await ApplicationData.Current.LocalFolder.ReadAsync<SuspendAndResumeArgs>(StateFilename);
            if (saveState?.Target != null && typeof(Page).IsAssignableFrom(saveState.Target))
            {
                return saveState;
            }

            return null;
        }
    }
}
