using System;
using Windows.UI.Xaml;
//PostActionAnchor: MVVM NAMESPACE

namespace ItemNamespace.WebViewPage
{
    public class WebViewPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private Visibility _loadingIndicatorVisibility;
        public Visibility LoadingIndicatorVisibility
        {
            get => _loadingIndicatorVisibility;
            set => Set(ref _loadingIndicatorVisibility, value);
        }

        private Uri _source;
        public Uri Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public WebViewPageViewModel()
        {
            LoadingIndicatorVisibility = Visibility.Visible;
            Source = new Uri("https://www.microsoft.com/");
        }
    }
}