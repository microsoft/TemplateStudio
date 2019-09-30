using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.UserActivities;
using Windows.UI.Core;
using Windows.UI.Shell;

namespace Param_RootNamespace.Services
{
    // More details about this functionality can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/user-activity.md
    // For more info about UserActivities in Timeline see https://docs.microsoft.com/windows/uwp/launch-resume/useractivities
    // For more info about UserActivities with AdaptiveCards see https://docs.microsoft.com/adaptive-cards/get-started/windows
    // Please note that user activities will only show on devices with Windows 10 Build 1803 or higher
    public static partial class UserActivityService
    {
        private static UserActivitySession _currentUserActivitySession;

        public static async Task CreateUserActivityAsync(UserActivityData activityData)
        {
            var activity = await activityData.ToUserActivity();

            // Cleanup any content assigned earlier
            activity.VisualElements.Content = null;
            await SaveAsync(activity);
        }

        public static async Task CreateUserActivityAsync(UserActivityData activityData, IAdaptiveCard adaptiveCard)
        {
            var activity = await activityData.ToUserActivity();

            activity.VisualElements.Content = adaptiveCard;
            await SaveAsync(activity);
        }

        private static async Task SaveAsync(UserActivity activity)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    await activity.SaveAsync();

                    // Dispose of any current UserActivitySession, and create a new one.
                    _currentUserActivitySession?.Dispose();
                    _currentUserActivitySession = activity.CreateSession();
                });
        }
    }
}
