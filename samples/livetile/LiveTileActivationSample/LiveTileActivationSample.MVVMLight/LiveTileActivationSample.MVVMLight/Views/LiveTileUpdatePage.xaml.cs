using System;

using LiveTileActivationSample.MVVMLight.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LiveTileActivationSample.MVVMLight.Views
{
    public sealed partial class LiveTileUpdatePage : Page
    {
        private LiveTileUpdateViewModel ViewModel
        {
            get { return DataContext as LiveTileUpdateViewModel; }
        }

        public LiveTileUpdatePage()
        {
            InitializeComponent();
        }
    }
}
