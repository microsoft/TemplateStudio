using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ItemNamespace.Services
{
    public class LocationService : ILocationService
    {
        private Geolocator geolocator;

        /// <inheritdoc />
        public event EventHandler<Geoposition> PositionChanged;

        /// <inheritdoc />
        public Geoposition CurrentPosition { get; private set; }

        /// <inheritdoc />
        public Task InitializeAsync()
        {
            return InitializeAsync(100);
        }

        /// <inheritdoc />
        public Task InitializeAsync(uint desiredAccuracyInMeters)
        {
            return InitializeAsync(desiredAccuracyInMeters, (double)desiredAccuracyInMeters / 2);
        }

        /// <inheritdoc />
        public async Task InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold)
        {
            if (geolocator != null)
            {
                geolocator.PositionChanged -= GeolocatorOnPositionChanged;
                geolocator = null;
            }

            var access = await Geolocator.RequestAccessAsync();

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:
                    geolocator = new Geolocator
                    {
                        DesiredAccuracyInMeters = desiredAccuracyInMeters,
                        MovementThreshold = movementThreshold
                    };
                    break;
            }
        }

        /// <inheritdoc />
        public async Task StartListeningAsync()
        {
            if (geolocator == null)
                throw new InvalidOperationException(
                    "The StartListening method cannot be called before the InitializeAsync method.");

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