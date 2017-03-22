using System;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;

namespace ItemNamespace.ViewModel
{
    public class MapPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private const double defaultZoomLevel = 19;
        private const double defaultLatitude = 47.639627;
        private const double defaultLongitude = -122.128227;

        private double _zoomLevel;
        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set { Set(ref _zoomLevel, value); }
        }

        private Geopoint _center;
        public Geopoint Center
        {
            get { return _center; }
            set { Set(ref _center, value); }
        }

        public MapPageViewModel()
        {
            ZoomLevel = defaultZoomLevel;
        }

        public void Initialize(MapControl map)
        {
            if (map == null)
            {
                return;
            }

            //TODO UWPTemplates: Set your map service token. If you don't have it, request at https://www.bingmapsportal.com/            
            map.MapServiceToken = "";

            var position = new BasicGeoposition() { Latitude = defaultLatitude, Longitude = defaultLongitude };
            Center = new Geopoint(position);
            AddMapIcon(map, Center, "Microsoft Corporation");
        }        

        private void AddMapIcon(MapControl map, Geopoint position, string title)
        {
            MapIcon mapIcon = new MapIcon()
            {
                Location = position,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = title,
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map.png")),
                ZIndex = 0
            };
            map.MapElements.Add(mapIcon);
        }
    }
}