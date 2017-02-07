using System;
using Windows.UI.Xaml;
#if (isBasic)
using Param_PageNS.Core;
#else if (isMVVMLight)
using GalaSoft.MvvmLight;
#endif

namespace Param_PageNS.WebViewPage
{
    public class WebViewPageViewModel : ViewModelBase
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