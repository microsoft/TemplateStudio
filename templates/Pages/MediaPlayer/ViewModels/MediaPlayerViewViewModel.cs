using System;
using System.Collections.ObjectModel;
using Windows.Media.Playback;
using Windows.Media.Core;

namespace Param_ItemNamespace.ViewModels
{
    public class MediaPlayerViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO [ML]: Replace with links to our build videos
        // TODO UWPTemplates: Set your video default and image here
        private const string defaultSource = "https://sec.ch9.ms/sessions/build/2016/B842_LG.mp4";
        // The poster image is displayed until the video is started
        private const string defaultPoster = "https://sec.ch9.ms/sessions/build/2016/B842.jpg";

        private IMediaPlaybackSource _source;
        public IMediaPlaybackSource Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        private string _posterSource;
        public string PosterSource
        {
            get { return _posterSource; }
            set { Set(ref _posterSource, value); }
        }

        public MediaPlayerViewViewModel()
        {
            Source = MediaSource.CreateFromUri(new Uri(defaultSource));
            PosterSource = defaultPoster;
        }
    }
}
