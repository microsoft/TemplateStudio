using System;
using Windows.UI.Xaml.Controls;

namespace ItemNamespace.Views
{
    public sealed partial class WebViewPagePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        //TODO UWPTemplates: Setup here your privacy web url
        private const string defaultUrl = "https://YourPrivacyUrlGoesHere/";

        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        public WebViewPagePage()
        {
            Source = new Uri(defaultUrl);
            InitializeComponent();
        }
    }
}
