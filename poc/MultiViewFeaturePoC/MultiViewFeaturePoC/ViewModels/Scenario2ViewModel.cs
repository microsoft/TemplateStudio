using System;

using MultiViewFeaturePoC.Helpers;
using MultiViewFeaturePoC.Services;
using Windows.UI.Core;
using MultiViewFeaturePoC.Views;
using Windows.UI.ViewManagement;

namespace MultiViewFeaturePoC.ViewModels
{
    public class Scenario2ViewModel : Observable
    {
        private const string SecondaryViewTitle = "Scenario2 secondary view";

        private ViewLifetimeControl _viewControl;
        private RelayCommand _openCommand;

        public RelayCommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(OnOpen, OnCanOpen));

        public Scenario2ViewModel()
        {
        }

        private async void OnOpen()
        {
            _viewControl = await WindowManagerService.TryShowAsViewModeAsync(SecondaryViewTitle, typeof(VideoPage), null, ApplicationViewMode.CompactOverlay ,OnClose);
            OpenCommand.OnCanExecuteChanged();
        }

        private bool OnCanOpen() => !WindowManagerService.IsWindowOpen(SecondaryViewTitle);

        private async void OnClose()
        {
            await _viewControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                VideoPage.Current.Pause();
            });            

            await WindowManagerService.RunOnMainThreadAsync(CoreDispatcherPriority.Normal, () =>
            {
                OpenCommand.OnCanExecuteChanged();
            });
        }
    }
}
