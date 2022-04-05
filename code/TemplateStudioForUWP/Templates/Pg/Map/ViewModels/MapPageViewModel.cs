using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.ViewModels
{
    public class MapPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO: Set your preferred default zoom level
        private const double DefaultZoomLevel = 17;

        private readonly LocationService _locationService;

        // TODO: Set your preferred default location if a geolock can't be found.
        private readonly BasicGeoposition _defaultPosition = new BasicGeoposition()
        {
            Latitude = 47.609425,
            Longitude = -122.3417
        };

        private double _zoomLevel;

        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set { Param_Setter(ref _zoomLevel, value); }
        }

        private Geopoint _center;

        public Geopoint Center
        {
            get { return _center; }
            set { Param_Setter(ref _center, value); }
        }

        public MapPageViewModel()
        {
            _locationService = new LocationService();
            Center = new Geopoint(_defaultPosition);
            ZoomLevel = DefaultZoomLevel;
        }

        public async Task InitializeAsync(MapControl map)
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged += LocationService_PositionChanged;

                var initializationSuccessful = await _locationService.InitializeAsync();

                if (initializationSuccessful)
                {
                    await _locationService.StartListeningAsync();
                }

                if (initializationSuccessful && _locationService.CurrentPosition != null)
                {
                    Center = _locationService.CurrentPosition.Coordinate.Point;
                }
                else
                {
                    Center = new Geopoint(_defaultPosition);
                }
            }

            if (map != null)
            {
                // TODO: Set your map service token. If you don't have one, request from https://www.bingmapsportal.com/
                // map.MapServiceToken = string.Empty;
                AddMapIcon(map, Center, "Map_YourLocation".GetLocalized());
            }
        }

        private void LocationService_PositionChanged(object sender, Geoposition geoposition)
        {
            if (geoposition != null)
            {
                Center = geoposition.Coordinate.Point;
            }
        }

        private void AddMapIcon(MapControl map, Geopoint position, string title)
        {
            var mapIcon = new MapIcon()
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
