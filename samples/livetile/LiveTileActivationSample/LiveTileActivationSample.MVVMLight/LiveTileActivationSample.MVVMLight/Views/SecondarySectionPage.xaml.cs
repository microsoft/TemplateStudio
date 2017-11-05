using System;

using LiveTileActivationSample.MVVMLight.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LiveTileActivationSample.MVVMLight.Views
{
    public sealed partial class SecondarySectionPage : Page
    {
        private SecondarySectionViewModel ViewModel
        {
            get { return DataContext as SecondarySectionViewModel; }
        }

        public SecondarySectionPage()
        {
            InitializeComponent();
        }
    }
}
