using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace ItemNamespace.MapPage
{
    public sealed partial class MapPagePage : Page
    {
        public MapPagePage()
        {
            this.InitializeComponent();
        }

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MapControl map = sender as MapControl;
            if (map == null)
            {
                throw new ArgumentNullException("Expected type is MapControl");
            }
            
            ViewModel.SetMap(map);
        }
    }
}
