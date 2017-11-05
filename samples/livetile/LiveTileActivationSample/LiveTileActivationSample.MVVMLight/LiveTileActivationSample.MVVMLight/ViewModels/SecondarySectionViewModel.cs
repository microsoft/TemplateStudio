using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveTileActivationSample.MVVMLight.Helpers;
using LiveTileActivationSample.MVVMLight.Services;

namespace LiveTileActivationSample.MVVMLight.ViewModels
{
    public class SecondarySectionViewModel : ViewModelBase
    {
        private ICommand _pinToStartCommand;
        public ICommand PinToStartCommand => _pinToStartCommand ?? (_pinToStartCommand = new RelayCommand(OnPinToStart));

        public SecondarySectionViewModel()
        {
        }

        private async void OnPinToStart()
        {
            await Singleton<LiveTileService>.Instance.SamplePinSecondaryAsync(LiveTileService.SecondarySectionPageID);
        }
    }
}
