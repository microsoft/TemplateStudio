using System;

namespace ItemNamespace.ViewModels
{
    public class WebViewPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        //TODO UWPTemplates: Setup here your privacy web url
        private const string defaultUrl = "https://YourPrivacyUrlGoesHere/";

        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        public WebViewPageViewModel()
        {
            Source = new Uri(defaultUrl);
        }
    }
}