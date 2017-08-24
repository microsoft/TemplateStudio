using System;
using VideoResume.MVVMBasic.Helpers;
using VideoResume.MVVMBasic.Services;
using VideoResume.MVVMBasic.ViewModels;

using Windows.Media.Playback;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VideoResume.MVVMBasic.Views
{
    public sealed partial class MediaPlayerPage : Page
    {
        public MediaPlayerViewModel ViewModel { get; } = new MediaPlayerViewModel();
        // For more on the MediaPlayer and adjusting controls and behavior see https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/media-playback
        // The DisplayRequest is used to stop the screen dimming while watching for extended periods
        private DisplayRequest _displayRequest = new DisplayRequest();
        private bool _isRequestActive = false;

        public MediaPlayerPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SuspensionState suspensionState = e.Parameter as SuspensionState;
            if (suspensionState != null)
            {
                bool success = TimeSpan.TryParse(suspensionState.Data?.ToString(), out TimeSpan position);
                if (success)
                {
                    mpe.MediaPlayer.PlaybackSession.Position = position;
                }
            }
            base.OnNavigatedTo(e);
            mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
            Singleton<SuspendAndResumeService>.Instance.OnBackgroundEntering += OnBackgroundEntering;
        }

        private void OnBackgroundEntering(object sender, OnBackgroundEnteringEventArgs e)
        {
            e.SuspensionState.Data = mpe.MediaPlayer.PlaybackSession.Position;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            mpe.MediaPlayer.Pause();
            mpe.MediaPlayer.PlaybackSession.PlaybackStateChanged -= PlaybackSession_PlaybackStateChanged;
            Singleton<SuspendAndResumeService>.Instance.OnBackgroundEntering -= OnBackgroundEntering;
        }

        private async void PlaybackSession_PlaybackStateChanged(MediaPlaybackSession sender, object args)
        {
            if (sender is MediaPlaybackSession playbackSession && playbackSession.NaturalVideoHeight != 0)
            {
                if (playbackSession.PlaybackState == MediaPlaybackState.Playing)
                {
                    if (!_isRequestActive)
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            _displayRequest.RequestActive();
                            _isRequestActive = true;
                        });
                    }
                }
                else
                {
                    if (_isRequestActive)
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            _displayRequest.RequestRelease();
                            _isRequestActive = false;
                        });
                    }
                }
            }
        }
    }
}
