using System;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;

namespace Page_NS.MVVMLightWebViewPage
{
    public class MVVMLightWebViewPageViewModel : ViewModelBase
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

        public MVVMLightWebViewPageViewModel()
        {
            LoadingIndicatorVisibility = Visibility.Visible;
            Source = new Uri("https://www.microsoft.com/");
        }
    }
}