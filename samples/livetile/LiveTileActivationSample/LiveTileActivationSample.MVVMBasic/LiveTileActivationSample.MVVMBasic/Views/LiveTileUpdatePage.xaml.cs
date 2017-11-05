using System;

using LiveTileActivationSample.MVVMBasic.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LiveTileActivationSample.MVVMBasic.Views
{
    public sealed partial class LiveTileUpdatePage : Page
    {
        public LiveTileUpdateViewModel ViewModel { get; } = new LiveTileUpdateViewModel();

        public LiveTileUpdatePage()
        {
            InitializeComponent();
        }
    }
}
