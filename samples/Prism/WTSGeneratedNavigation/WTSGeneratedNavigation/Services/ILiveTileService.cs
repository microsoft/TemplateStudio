using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace WTSGeneratedNavigation.Services
{
    internal interface ILiveTileService
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
