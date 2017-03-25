using System;

namespace ItemNamespace.ViewModel
{
    public class WebViewViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        //TODO UWPTemplates: Setup here your web url
        private const string defaultUrl = "https://YourPrivacyStatementUrl/";

        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        public WebViewViewViewModel()
        {
            Source = new Uri(defaultUrl);
        }
    }
}