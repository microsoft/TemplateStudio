using System;

using LiveTileActivationSample.MVVMLight.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LiveTileActivationSample.MVVMLight.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
