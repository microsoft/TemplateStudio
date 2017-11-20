using System;

using Windows.UI.Core;

using MultiViewFeaturePoC.Helpers;
using MultiViewFeaturePoC.Services;
using MultiViewFeaturePoC.Views;

namespace MultiViewFeaturePoC.ViewModels
{
    public class Scenario1ViewModel : Observable
    {
        public static Scenario1ViewModel Current;
        private const string SecondaryViewTitle = "Scenario1 secondary view";

        private ViewLifetimeControl _viewControl;

        private RelayCommand _openCommand;
        private RelayCommand _updateCommand;
        private string _timeString = "not jet!";

        public RelayCommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(OnOpen, OnCanOpen));

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

        private bool OnCanOpen() => !WindowManagerService.Current.IsWindowOpen(SecondaryViewTitle);
        private bool OnCanUpdate() => WindowManagerService.Current.IsWindowOpen(SecondaryViewTitle);

        private async void OnOpen()
        {
            _viewControl = await WindowManagerService.Current.TryShowAsStandaloneAsync(SecondaryViewTitle, typeof(Scenario1SecondaryPage));
            _viewControl.Released += _viewControl_Released;
            OpenCommand.OnCanExecuteChanged();
            UpdateCommand.OnCanExecuteChanged();
        }

        private async void _viewControl_Released(object sender, EventArgs e)
        {
            ((ViewLifetimeControl)sender).Released -= _viewControl_Released;
            await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OpenCommand.OnCanExecuteChanged();
                UpdateCommand.OnCanExecuteChanged();
            });
        }
    }
}
