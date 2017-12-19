using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WTSPrism.Services
{
    public interface ILocationService
    {
        Geoposition CurrentPosition { get; }

        event EventHandler<Geoposition> PositionChanged;

        Task<bool> InitializeAsync();
        Task<bool> InitializeAsync(uint desiredAccuracyInMeters);
        Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold);
        Task StartListeningAsync();
        void StopListening();
    }
}