//{[{
using System.Threading.Tasks;
using Param_ItemNamespace.Helpers;
//}]}

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
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