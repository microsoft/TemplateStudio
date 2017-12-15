using System;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

using WTSGeneratedPivot.Helpers;

namespace WTSGeneratedPivot.Services
{
    public class LocationService : ILocationService
    {
        private Geolocator geolocator;

        /// <summary>
        /// Raised when the current position is updated.
        /// </summary>
        public event EventHandler<Geoposition> PositionChanged;

        /// <summary>
        /// Gets the last known recorded position.
        /// </summary>
        public Geoposition CurrentPosition { get; private set; }

        /// <summary>
        /// Initializes the location service with a default accuracy (100 meters) and movement threshold.
        /// </summary>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        public Task<bool> InitializeAsync()
        {
            return InitializeAsync(100);
        }

        /// <summary>
        /// Initializes the location service with the specified accuracy and default movement threshold.
        /// </summary>
        /// <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        public Task<bool> InitializeAsync(uint desiredAccuracyInMeters)
        {
            return InitializeAsync(desiredAccuracyInMeters, (double)desiredAccuracyInMeters / 2);
        }

        /// <summary>
        /// Initializes the location service with the specified accuracy and movement threshold.
        /// </summary>
        /// <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        /// <param name="movementThreshold">The distance of movement, in meters, that is required for the service to raise the PositionChanged event.</param>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        public async Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold)
        {
            // to find out more about getting location, go to https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/get-location
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

        /// <summary>
        /// Starts the service listening for location updates.
        /// </summary>
        /// <returns>An object that is used to manage the asynchronous operation.</returns>
        public async Task StartListeningAsync()
        {
            if (geolocator == null)
            {
                throw new InvalidOperationException(
                    "The StartListening method cannot be called before the InitializeAsync method is called and returns true.");
            }

            geolocator.PositionChanged += GeolocatorOnPositionChanged;

            CurrentPosition = await geolocator.GetGeopositionAsync();
        }

        /// <summary>
        /// Stops the service listening for location updates.
        /// </summary>
        public void StopListening()
        {
            if (geolocator == null)
            {
                return;
            }

            geolocator.PositionChanged -= GeolocatorOnPositionChanged;
        }

        private async void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            CurrentPosition = args.Position;

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PositionChanged?.Invoke(this, CurrentPosition);
            });
        }
    }
}
