using System;

using Windows.UI.Xaml.Controls;

using WTSGeneratedNavigation.ViewModels;

namespace WTSGeneratedNavigation.Views
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
