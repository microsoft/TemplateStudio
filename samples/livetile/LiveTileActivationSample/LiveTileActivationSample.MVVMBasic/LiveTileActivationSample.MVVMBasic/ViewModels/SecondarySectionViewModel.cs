using System;
using System.Windows.Input;
using LiveTileActivationSample.MVVMBasic.Helpers;
using LiveTileActivationSample.MVVMBasic.Services;

namespace LiveTileActivationSample.MVVMBasic.ViewModels
{
    public class SecondarySectionViewModel : Observable
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
