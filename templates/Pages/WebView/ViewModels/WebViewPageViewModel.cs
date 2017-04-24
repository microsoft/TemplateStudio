using System;

namespace Param_ItemNamespace.ViewModels
{
    public class WebViewPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO UWPTemplates: Set your hyperlink default here
        private const string defaultUrl = "https://developer.microsoft.com/en-us/windows/apps";

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
