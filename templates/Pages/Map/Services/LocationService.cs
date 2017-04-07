using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ItemNamespace.Services
{
    public class LocationService
    {
        private Geolocator geolocator;

        /// <inheritdoc />
        public event EventHandler<Geoposition> PositionChanged;

        /// <inheritdoc />
        public Geoposition CurrentPosition { get; private set; }

        /// <inheritdoc />
        public Task<bool> InitializeAsync()
        {
            return InitializeAsync(100);
        }

        /// <inheritdoc />
        public Task<bool> InitializeAsync(uint desiredAccuracyInMeters)
        {
            return InitializeAsync(desiredAccuracyInMeters, (double)desiredAccuracyInMeters / 2);
        }

        /// <inheritdoc />
        public async Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold)
        {
            if (geolocator != null)
            {
                geolocator.PositionChanged -= GeolocatorOnPositionChanged;
                geolocator = null;
            }

            var access = await Geolocator.RequestAccessAsync();

            bool result;

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:
                    geolocator = new Geolocator
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

        /// <inheritdoc />
        public async Task StartListeningAsync()
        {
            if (geolocator == null)
                throw new InvalidOperationException(
                    "The StartListening method cannot be called before the InitializeAsync method is called and returns true.");

            geolocator.PositionChanged += GeolocatorOnPositionChanged;

            CurrentPosition = await geolocator.GetGeopositionAsync();
        }

        /// <inheritdoc />
        public void StopListening()
        {
            if (geolocator == null) return;

            geolocator.PositionChanged -= GeolocatorOnPositionChanged;
        }

        private async void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args == null) return;

            CurrentPosition = args.Position;

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PositionChanged?.Invoke(this, CurrentPosition);
            });
        }
    }
}