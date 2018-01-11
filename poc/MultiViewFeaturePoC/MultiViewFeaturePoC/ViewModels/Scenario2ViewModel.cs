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
            _viewControl = await WindowManagerService.Current.TryShowAsViewModeAsync(SecondaryViewTitle, typeof(VideoPage), ApplicationViewMode.CompactOverlay);
            _viewControl.Released += _viewControl_Released;
            OpenCommand.OnCanExecuteChanged();
        }

        private async void _viewControl_Released(object sender, EventArgs e)
        {
            ((ViewLifetimeControl)sender).Released -= _viewControl_Released;
            VideoPage.Current.Pause();
            await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OpenCommand.OnCanExecuteChanged();
            });
        }

        private bool OnCanOpen() => !WindowManagerService.Current.IsWindowOpen(SecondaryViewTitle);
    }
}
