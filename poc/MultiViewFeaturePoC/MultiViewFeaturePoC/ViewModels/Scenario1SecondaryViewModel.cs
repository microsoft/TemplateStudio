using System;

using MultiViewFeaturePoC.Helpers;
using MultiViewFeaturePoC.Services;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace MultiViewFeaturePoC.ViewModels
{
    public class Scenario1SecondaryViewModel : Observable
    {
        public static Scenario1SecondaryViewModel Current;

        private ViewLifetimeControl _viewLifetimeControl;
        private int _mainViewId;
        private CoreDispatcher _mainDispatcher;
        private string _timeString = "not jet!";
        private RelayCommand _updateCommand;

        public string TimeString
        {
            get => _timeString;
            set => Set(ref _timeString, value);
        }

        public RelayCommand UpdateCommand => _updateCommand ?? (_updateCommand = new RelayCommand(OnUpdate));

        public Scenario1SecondaryViewModel()
        {
            Current = this;
        }

        internal void Initialize(ViewLifetimeControl viewLifetimeControl)
        {
            _viewLifetimeControl = viewLifetimeControl;
            _mainViewId = WindowManagerService.Current.MainViewId;
            _mainDispatcher = WindowManagerService.Current.MainDispatcher;
            _viewLifetimeControl.Released += ViewLifetimeControl_Released;
        }

        private async void ViewLifetimeControl_Released(object sender, EventArgs e)
        {
            ((ViewLifetimeControl)sender).Released -= ViewLifetimeControl_Released;
            await _mainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                WindowManagerService.Current.SecondaryViews.Remove(_viewLifetimeControl);
            });
            Window.Current.Close();
        }

        private async void OnUpdate()
        {
            await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Scenario1ViewModel.Current.TimeString = DateTime.Now.ToString();
            });
        }
    }
}
