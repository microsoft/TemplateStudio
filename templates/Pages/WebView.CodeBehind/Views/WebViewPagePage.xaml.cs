using System;
using Windows.UI.Xaml.Controls;

namespace ItemNamespace.Views
{
    public sealed partial class WebViewPagePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        //TODO UWPTemplates: Set your hyperlink default here
        private const string defaultUrl = "https://YourUrlGoesHere/";

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
