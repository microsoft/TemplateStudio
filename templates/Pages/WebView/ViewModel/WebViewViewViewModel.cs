using System;

namespace ItemNamespace.ViewModel
{
    public class WebViewViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        public WebViewViewViewModel()
        {
        }

        public void Initialize()
        {
            //TODO UWPTemplates: Setup here your web url
            Source = new Uri("https://www.microsoft.com/");   
        }
    }
}