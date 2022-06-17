using System;
using System.Threading.Tasks;

using Param_ProjectName.Contracts.Services;
using Param_ProjectName.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;

namespace Param_RootNamespace.Activation
{
    public class NotificationActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        private readonly INavigationService _navigationService;

        public NotificationActivationHandler(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            return AppInstance.GetCurrent().GetActivatedEventArgs()?.Kind == ExtendedActivationKind.AppNotification;
        }

        protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            AppActivationArguments activationArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
            var notificationActivatedEventArgs = (AppNotificationActivatedEventArgs)activationArgs.Data;
            var action = ExtractParamFromArgs(notificationActivatedEventArgs.Argument, "action");

            switch (action)
            {
                case "CGridPage":
                    _navigationService.NavigateTo(typeof(ContentGridViewModel).FullName);
                    break;

                default:
                    _navigationService.NavigateTo(typeof(MainViewModel).FullName);
                    break;
            }
            await Task.CompletedTask;
        }

        public static string ExtractParamFromArgs(string args, string paramName)
        {
            var tag = paramName;
            tag += "=";

            var tagStart = args.IndexOf(tag);
            if (tagStart == -1)
            {
                return null;
            }

            var paramStart = tagStart + tag.Length;

            var paramEnd = args.IndexOf("&", paramStart);
            if (paramEnd == -1)
            {
                paramEnd = args.Length;
            }

            return args.Substring(paramStart, paramEnd - paramStart);
        }
    }
}
