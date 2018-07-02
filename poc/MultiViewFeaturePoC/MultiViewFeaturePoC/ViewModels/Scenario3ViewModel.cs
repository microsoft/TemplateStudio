using System;

using MultiViewFeaturePoC.Helpers;
using MultiViewFeaturePoC.Services;
using MultiViewFeaturePoC.Views;

namespace MultiViewFeaturePoC.ViewModels
{
    public class Scenario3ViewModel : Observable
    {
        private int _opened = 0;
        private string _openedPages = "0 Pages";
        private RelayCommand _openCommand;

        private string SecondaryViewTitle => $"Scenario3 secondary view {DateTime.Now.ToString()}";

        public string OpenedPages
        {
            get => _openedPages;
            set => Set(ref _openedPages, value);
        }

        public RelayCommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(OnOpen));

        private async void OnOpen()
        {
            var viewControl = await WindowManagerService.Current.TryShowAsStandaloneAsync(SecondaryViewTitle, typeof(Scenario3SecondaryPage));
            viewControl.Released += _viewControl_Released;
            _opened++;
            OpenedPages = $"Opened pages {_opened}";
        }

        private async void _viewControl_Released(object sender, EventArgs e)
        {
            ((ViewLifetimeControl)sender).Released -= _viewControl_Released;
            await WindowManagerService.Current.MainDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _opened--;
                OpenedPages = $"Opened pages {_opened}";
            });
        }

        public Scenario3ViewModel()
        {
        }
    }
}
