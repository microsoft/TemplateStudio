using System;
using System.Collections.ObjectModel;

using Prism.Windows.Mvvm;

using Windows.Media.Core;
using Windows.Media.Playback;

namespace WTSPrismNavigationBase.ViewModels
{
    public class MediaPlayerPageViewModel : ViewModelBase
    {
        // TODO WTS: Set your video default and image here
        private const string DefaultSource = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_high.mp4";
        // The poster image is displayed until the video is started
        private const string DefaultPoster = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_960.jpg";

        private IMediaPlaybackSource source;
        public IMediaPlaybackSource Source
        {
            get { return source; }
            set { SetProperty(ref source, value); }
        }

        private string posterSource;
        public string PosterSource
        {
            get { return posterSource; }
            set { SetProperty(ref posterSource, value); }
        }

        public MediaPlayerPageViewModel()
        {
            Source = MediaSource.CreateFromUri(new Uri(DefaultSource));
            PosterSource = DefaultPoster;
        }
    }
}

