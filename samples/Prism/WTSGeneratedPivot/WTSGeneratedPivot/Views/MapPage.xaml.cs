using System;

using Windows.UI.Xaml.Controls;

using WTSGeneratedPivot.ViewModels;

namespace WTSGeneratedPivot.Views
{
    public sealed partial class MapPage : Page
    {
        private MapViewModel ViewModel => DataContext as MapViewModel;

        public MapPage()
        {
            InitializeComponent();
        }
    }
}
