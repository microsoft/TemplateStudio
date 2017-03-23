using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ItemNamespace.View
{
    public sealed partial class WebViewViewView : Page
    {
        public WebViewViewView()
        {
            this.InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                throw new ArgumentNullException("ViewModel");
            }
            
            ViewModel.Initialize();
        }
    }
}
