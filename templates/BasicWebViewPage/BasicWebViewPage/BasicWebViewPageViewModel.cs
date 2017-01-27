using System;
using Windows.UI.Xaml;
using Page_NS.Core;

namespace Page_NS.BasicWebViewPage
{
    public class BasicWebViewPageViewModel : ViewModelBase
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

        public BasicWebViewPageViewModel()
        {
            LoadingIndicatorVisibility = Visibility.Visible;
            Source = new Uri("https://www.microsoft.com/");
        }
    }
}