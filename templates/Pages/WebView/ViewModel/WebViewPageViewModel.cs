using System;

namespace ItemNamespace.ViewModel
{
    public class WebViewPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        public WebViewPageViewModel()
        {
        }

        public void Initialize()
        {
            //TODO UWPTemplates: Setup here your web url
            Source = new Uri("https://www.microsoft.com/");   
        }
    }
}