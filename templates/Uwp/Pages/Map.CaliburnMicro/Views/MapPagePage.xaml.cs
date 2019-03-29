using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace Param_RootNamespace.Views
{
    public sealed partial class MapPagePage : Page, IMapPageView
    {
        public MapPagePage()
        {
            InitializeComponent();
        }

        public void AddMapIcon(Geopoint position, string title)
        {
            var mapIcon = new MapIcon()
            {
                Location = position,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = title,
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map.png")),
                ZIndex = 0
            };

            mapControl.MapElements.Add(mapIcon);
        }
    }
}
