//{[{
using System.Reflection;
using Windows.UI.Xaml.Controls;
//}]}
namespace Param_RootNamespace.Services
{
    internal class SuspendAndResumeService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private const string StateFilename = "SuspendAndResumeState";

        //{[{
        // TODO: Subscribe to the OnBackgroundEntering and OnDataRestored events from your current Page to save and restore the current App data.
        // Only one Page should subscribe to OnBackgroundEntering and OnDataRestored at a time, as the App will navigate to that Page on resume.
        public event EventHandler<SuspendAndResumeArgs> OnBackgroundEntering;

        public event EventHandler<SuspendAndResumeArgs> OnDataRestored;

        // TODO: Subscribe to the OnResuming event from the current Page
        // if you need to refresh online data when the App resumes without being terminated.
        public event EventHandler OnResuming;
        //}]}
        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            //^^
            //{[{
            if (saveState?.Target != null && typeof(Page).IsAssignableFrom(saveState.Target))
            {
                NavigationService.Navigate(saveState.Target, saveState.SuspensionState);
            }
            //}]}
        }
    }
}
