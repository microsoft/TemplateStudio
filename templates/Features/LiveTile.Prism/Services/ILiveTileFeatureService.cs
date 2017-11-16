using Windows.UI.Notifications;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Windows.ApplicationModel.Activation;

namespace Param_RootNamespace.Services
{
    internal interface ILiveTileFeatureService
    {
        Task EnableQueueAsync();

        void UpdateTile(TileNotification notification);

        Task<bool> PinSecondaryTileAsync(SecondaryTile tile, bool allowDuplicity = false);

        Task HandleInternalAsync(LaunchActivatedEventArgs args);

        bool CanHandleInternal(LaunchActivatedEventArgs args);

        void SampleUpdate();

        Task SamplePinSecondaryAsync(string pageName);
    }
}
