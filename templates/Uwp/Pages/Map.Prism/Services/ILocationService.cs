using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Param_RootNamespace.Helpers;

namespace Param_RootNamespace.Services
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
