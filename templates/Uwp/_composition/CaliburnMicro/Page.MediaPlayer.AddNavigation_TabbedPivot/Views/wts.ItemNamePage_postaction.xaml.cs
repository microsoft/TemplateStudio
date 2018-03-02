//{[{
using System.Linq;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Caliburn.Micro;
//}]}

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
            //{[{
            Loaded += wts.ItemNamePage_Loaded;
            //}]}
        }

        //{[{
        private void wts.ItemNamePage_Loaded(object sender, RoutedEventArgs e)
        {
            var element = this as FrameworkElement;
            var pivotPage = element.FindAscendant<Pivot>();

            if (pivotPage != null)
            {
                pivotPage.SelectionChanged += PivotPage_SelectionChanged;
            }

            mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
            mpe.MediaPlayer.Play();
        }

        private void PivotPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool navigatedTo = e.AddedItems.Cast<Screen>().Any(p => p.GetView() is wts.ItemNamePage);
            bool navigatedFrom = e.RemovedItems.Cast<Screen>().Any(p => p.GetView() is wts.ItemNamePage);

            if (navigatedTo)
            {
                mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
            }

            if (navigatedFrom)
            {
                mpe.MediaPlayer.Pause();
                mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged -= PlaybackSession_PlaybackStateChanged;
            }
        }
        //}]}
    }
}