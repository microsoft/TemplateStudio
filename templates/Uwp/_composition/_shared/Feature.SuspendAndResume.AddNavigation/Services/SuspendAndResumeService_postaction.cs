//{[{
using System.Reflection;
using Windows.UI.Xaml.Controls;
//}]}
namespace Param_ItemNamespace.Services
{
    internal class SuspendAndResumeService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private const string StateFilename = "SuspendAndResumeState";
        //^^
        //{[{
        // TODO WTS: Subscribe to the OnBackgroundEntering event from your current Page to save the current App data.
        // Only one Page should subscribe to OnBackgroundEntering at a time, as app will navigate to that Page on resume.
        public event EventHandler<OnBackgroundEnteringEventArgs> OnBackgroundEntering;

        // TODO WTS: Subscribe to the OnResuming event from the current Page
        // if you need to refresh online data when the app resumes without being terminated.
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
