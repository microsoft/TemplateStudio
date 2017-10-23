using System;

using ToastNotificationSample.Helpers;
using Windows.ApplicationModel.Activation;

namespace ToastNotificationSample.ViewModels
{
    public class ActivatedFromToastViewModel : Observable
    {
        private string _activationSource;

        public string ActivationSource
        {
            get { return _activationSource; }
            set { Set(ref _activationSource, value); }
        }

        public ActivatedFromToastViewModel()
        {
        }

        public void Initialize(ToastNotificationActivatedEventArgs args)
        {
            // args.Argument contains information specified on Toast notification creation in file ToastNotificationsService.Samples.cs
            if (args.Argument == "ToastButtonActivationArguments")
            {
                // The application was launched by clicking on OK button of Toast Notification
                ActivationSource = "ActivationSourceButtonOk".GetLocalized();
            }
            else if(args.Argument == "ToastContentActivationParams")
            {
                // The application was launched by clicking on the main body of Toast Notification
                ActivationSource = "ActivationSourceContent".GetLocalized();
            }
        }
    }
}
