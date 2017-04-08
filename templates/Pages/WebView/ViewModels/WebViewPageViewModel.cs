using System;

namespace ItemNamespace.ViewModels
{
    public class WebViewPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        //TODO UWPTemplates: Set your hyperlink default here
        private const string defaultUrl = "https://YourUrlGoesHere/";

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