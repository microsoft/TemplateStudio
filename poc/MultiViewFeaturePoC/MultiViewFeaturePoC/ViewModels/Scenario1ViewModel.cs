using System;

using MultiViewFeaturePoC.Helpers;
using MultiViewFeaturePoC.Services;
using Windows.UI.Core;
using MultiViewFeaturePoC.Views;

namespace MultiViewFeaturePoC.ViewModels
{
    public class Scenario1ViewModel : Observable
    {
        public static Scenario1ViewModel Current;
        private const string SecondaryViewTitle = "Scenario1 secondary view";

        private RelayCommand _openCommand;
        private RelayCommand _updateCommand;
        private string _timeString = "not jet!";
        private ViewLifetimeControl _viewControl;

        public RelayCommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(OnOpen));

        public RelayCommand UpdateCommand => _updateCommand ?? (_updateCommand = new RelayCommand(OnUpdate, OnCanUpdate));

        public string TimeString
        {
            get => _timeString;
            set => Set(ref _timeString, value);
        }

        public Scenario1ViewModel()
        {
            Current = this;
        }

        private async void OnUpdate()
        {
            await _viewControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Scenario1SecondaryViewModel.Current.TimeString = DateTime.Now.ToString();
            });            
        }

        private bool OnCanUpdate() => WindowManagerService.IsWindowOpen(SecondaryViewTitle);

        private async void OnOpen()
        {
            _viewControl = await WindowManagerService.TryShowAsStandaloneAsync(SecondaryViewTitle, typeof(Scenario1SecondaryPage), null, OnClose);
            OpenCommand.OnCanExecuteChanged();
            UpdateCommand.OnCanExecuteChanged();
        }

        private async void OnClose()
        {
            await WindowManagerService.RunOnMainThreadAsync(CoreDispatcherPriority.Normal, () =>
            {
                OpenCommand.OnCanExecuteChanged();
                UpdateCommand.OnCanExecuteChanged();
            });
        }
    }
}
