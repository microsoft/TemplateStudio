using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using ItemNamespace.Services;

namespace ItemNamespace.ViewModels
{
    public class MapPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO UWPTemplates: Set your perferred default zoom level
        private const double defaultZoomLevel = 17;

        private readonly LocationService locationService;

        // TODO UWPTemplates: Set your perferred default location if a geolock can't be found.
        private readonly BasicGeoposition defaultPosition = new BasicGeoposition()
        {
            Latitude = 47.609425, 
            Longitude = -122.3417
        };

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
            locationService = new LocationService();
            Center = new Geopoint(defaultPosition);
            ZoomLevel = defaultZoomLevel;
        }

        public async Task InitializeAsync(MapControl map)
        {
            if (locationService != null)
            {
                locationService.PositionChanged += LocationServicePositionChanged;

                var initializationSuccessful = await locationService.InitializeAsync();

                if (initializationSuccessful)
                {
                    await locationService.StartListeningAsync();
                }

                if (initializationSuccessful && locationService.CurrentPosition != null)
                {
                    Center = locationService.CurrentPosition.Coordinate.Point;
                }
                else
                {
                    Center = new Geopoint(defaultPosition);
                }
            }

            if (map != null)
            {
                //TODO UWPTemplates: Set your map service token. If you don't have it, request at https://www.bingmapsportal.com/            
                map.MapServiceToken = "";

                AddMapIcon(map, Center, "Your location");
            }
        }

        public void Cleanup()
        {
            if (locationService != null)
            {
                locationService.PositionChanged -= LocationServicePositionChanged;
                locationService.StopListening();
            }
        }

        private void LocationServicePositionChanged(object sender, Geoposition geoposition)
        {
            if (geoposition != null)
            {
                Center = geoposition.Coordinate.Point;
            }
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