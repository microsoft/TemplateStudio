//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Services
{
    internal class SuspendAndResumeService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private const string StateFilename = "SuspendAndResumeState";

        //{[{
        // TODO WTS: Subscribe to the OnBackgroundEntering and OnDataRestored events from your current Page to save and restore the current App data.
        // Only one Page should subscribe to OnBackgroundEntering and OnDataRestored at a time, as the App will navigate to that Page on resume.
        public event EventHandler<SuspendAndResumeArgs> OnBackgroundEntering;

        public event EventHandler<SuspendAndResumeArgs> OnDataRestored;

        // TODO WTS: Subscribe to the OnResuming event from the current Page's ViewModel
        // if you need to refresh online data when the App resumes without being terminated.
        public event EventHandler OnResuming;
        //}]}
        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            //^^
            //{[{
            if (saveState?.Target != null)
            {
                ViewModelLocator.Current.NavigationService.Navigate(saveState.Target.FullName, saveState.SuspensionState);
            }
            //}]}
        }
    }
}
