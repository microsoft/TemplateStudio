using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserActivities;
using Windows.UI;
using Param_RootNamespace.Activation;

namespace Param_RootNamespace.Services
{
    public class UserActivityData
    {
        public string ActivityId { get; private set; }

        public SchemeActivationData ActivationData { get; private set; }

        public string DisplayText { get; private set; }

        public string Description { get; private set; }

        public Color BackgroundColor { get; private set; }

        public UserActivityData(string activityId, SchemeActivationData activationData, string displayText, Color backgroundColor = default, string description = null)
        {
            ActivityId = activityId;
            ActivationData = activationData;
            DisplayText = displayText;
            BackgroundColor = backgroundColor;
            Description = description ?? string.Empty;
        }

        public async Task<UserActivity> ToUserActivity()
        {
            if (string.IsNullOrEmpty(ActivityId))
            {
                throw new ArgumentNullException(nameof(ActivityId));
            }
            else if (ActivationData == null)
            {
                throw new ArgumentNullException(nameof(ActivationData));
            }
            else if (string.IsNullOrEmpty(DisplayText))
            {
                throw new ArgumentNullException(nameof(DisplayText));
            }

            var channel = UserActivityChannel.GetDefault();

            var activity = await channel.GetOrCreateUserActivityAsync(ActivityId);
            activity.ActivationUri = ActivationData.Uri;
            activity.VisualElements.DisplayText = DisplayText;
            activity.VisualElements.BackgroundColor = BackgroundColor;
            activity.VisualElements.Description = Description;

            return activity;
        }
    }
}