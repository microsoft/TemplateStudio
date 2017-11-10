using System;

using MultiViewFeaturePoC.Helpers;

namespace MultiViewFeaturePoC.ViewModels
{
    public class Scenario1SecondaryViewModel : Observable
    {
        public static Scenario1SecondaryViewModel Current;

        private string _timeString = "not jet!";
        private RelayCommand _updateCommand;

        public string TimeString
        {
            get => _timeString;
            set => Set(ref _timeString, value);
        }

        public RelayCommand UpdateCommand => _updateCommand ?? (_updateCommand = new RelayCommand(OnUpdate));
        
        private async void OnUpdate()
        {
            await Services.WindowManagerService.RunOnMainThreadAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Scenario1ViewModel.Current.TimeString = DateTime.Now.ToString();
            });
        }

        public Scenario1SecondaryViewModel()
        {
            Current = this;
        }
    }
}
