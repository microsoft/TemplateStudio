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
            if (this.geolocator != null)
            {
                this.geolocator.PositionChanged -= GeolocatorOnPositionChanged;
                this.geolocator = null;
            }

            var access = await Geolocator.RequestAccessAsync();

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:
                    this.geolocator = new Geolocator
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
            if (this.geolocator == null)
                throw new InvalidOperationException(
                    "The StartListening method cannot be called before the InitializeAsync method.");

            this.geolocator.PositionChanged += GeolocatorOnPositionChanged;

            this.CurrentPosition = await this.geolocator.GetGeopositionAsync();
        }

        /// <inheritdoc />
        public void StopListening()
        {
            if (this.geolocator == null) return;

            this.geolocator.PositionChanged -= GeolocatorOnPositionChanged;
        }

        private async void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args == null) return;

            this.CurrentPosition = args.Position;

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.PositionChanged?.Invoke(this, this.CurrentPosition);
            });
        }
    }
}