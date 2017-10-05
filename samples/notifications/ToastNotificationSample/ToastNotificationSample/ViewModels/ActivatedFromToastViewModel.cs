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
            get => _activationSource;
            set => Set(ref _activationSource, value);
        }

        public ActivatedFromToastViewModel()
        {
        }

        public void Initialize(ToastNotificationActivatedEventArgs args)
        {
            if (args.Argument == "ToastButtonActivationArguments")
            {
                ActivationSource = "ActivationSourceButtonOk".GetLocalized();
            }
            else if(args.Argument == "ToastContentActivationParams")
            {
                ActivationSource = "ActivationSourceContent".GetLocalized();
            }
        }
    }
}
