using System;

using LiveTileActivationSample.MVVMBasic.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LiveTileActivationSample.MVVMBasic.Views
{
    public sealed partial class SecondarySectionPage : Page
    {
        public SecondarySectionViewModel ViewModel { get; } = new SecondarySectionViewModel();

        public SecondarySectionPage()
        {
            InitializeComponent();
        }
    }
}
