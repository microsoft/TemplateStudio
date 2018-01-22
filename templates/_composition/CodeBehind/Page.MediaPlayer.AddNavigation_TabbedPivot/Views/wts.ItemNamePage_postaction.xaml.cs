//{[{
using System.Linq;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Extensions;
//}]}

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
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
            bool navigatedTo = e.AddedItems.Cast<PivotItem>().Any(p => p.FindDescendant<wts.ItemNamePage>() != null);
            bool navigatedFrom = e.RemovedItems.Cast<PivotItem>().Any(p => p.FindDescendant<wts.ItemNamePage>() != null);

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