using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.Devices.Geolocation;
using Param_RootNamespace.Helpers;

namespace Param_RootNamespace.Services
{
    public class LocationService : ILocationService
    {
        private Geolocator _geolocator;

        public event EventHandler<Geoposition> PositionChanged;

        public Geoposition CurrentPosition { get; private set; }

        public Task<bool> InitializeAsync()
        {
            return InitializeAsync(100);
        }

        public Task<bool> InitializeAsync(uint desiredAccuracyInMeters)
        {
            return InitializeAsync(desiredAccuracyInMeters, (double)desiredAccuracyInMeters / 2);
        }

        public async Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold)
        {
            // More about getting location at https://docs.microsoft.com/windows/uwp/maps-and-location/get-location
            if (_geolocator != null)
            {
                _geolocator.PositionChanged -= GeolocatorOnPositionChanged;
                _geolocator = null;
            }

            var access = await Geolocator.RequestAccessAsync();

            bool result;

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:
                    _geolocator = new Geolocator
                    {
                        DesiredAccuracyInMeters = desiredAccuracyInMeters,
                        MovementThreshold = movementThreshold
                    };
                    result = true;
                    break;
                case GeolocationAccessStatus.Unspecified:
                case GeolocationAccessStatus.Denied:
                default:
                    result = false;
                    break;
            }

            return result;
        }

        public async Task StartListeningAsync()
        {
            if (_geolocator == null)
            {
                throw new InvalidOperationException("ExceptionLocationServiceStartListeningCanNotBeCalled".GetLocalized());
            }

            _geolocator.PositionChanged += GeolocatorOnPositionChanged;

            CurrentPosition = await _geolocator.GetGeopositionAsync();
        }

        public void StopListening()
        {
            if (_geolocator == null)
            {
                return;
            }

            _geolocator.PositionChanged -= GeolocatorOnPositionChanged;
        }

        private async void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            CurrentPosition = args.Position;

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PositionChanged?.Invoke(this, CurrentPosition);
            });
        }
    }
}
