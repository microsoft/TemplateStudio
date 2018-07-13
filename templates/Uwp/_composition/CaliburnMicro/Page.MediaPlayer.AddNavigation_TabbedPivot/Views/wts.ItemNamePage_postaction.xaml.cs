//{[{
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Param_ItemNamespace.Helpers;
//}]}

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
        }

        //{[{
        public async Task OnPivotSelectedAsync()
        {
            mpe.MediaPlayer.Play();
            mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
            await Task.CompletedTask;
        }

        public async Task OnPivotUnselectedAsync()
        {
            mpe.MediaPlayer.Pause();
            mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged -= PlaybackSession_PlaybackStateChanged;
            await Task.CompletedTask;
        }
        //}]}
    }
}