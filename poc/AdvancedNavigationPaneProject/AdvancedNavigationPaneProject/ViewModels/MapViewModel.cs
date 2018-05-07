using System;
using System.Threading.Tasks;
using System.Windows.Input;

using AdvancedNavigationPaneProject.Helpers;
using AdvancedNavigationPaneProject.Services;
using AdvancedNavigationPaneProject.Views;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;

namespace AdvancedNavigationPaneProject.ViewModels
{
    public class MapViewModel : Observable
    {
        // TODO WTS: Set your preferred default zoom level
        private const double DefaultZoomLevel = 17;

        private readonly LocationService _locationService;

        // TODO WTS: Set your preferred default location if a geolock can't be found.
        private readonly BasicGeoposition _defaultPosition = new BasicGeoposition()
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

        private ICommand _expandCommand;

        public ICommand ExpandCommand => _expandCommand ?? (_expandCommand = new RelayCommand(OnExpand, CanExpand));

        public MapViewModel()
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
                // TODO WTS: Set your map service token. If you don't have one, request from https://www.bingmapsportal.com/
                //map.MapServiceToken = string.Empty;

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

        public void Cleanup()
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationService_PositionChanged;
                _locationService.StopListening();
            }
        }

        private void OnExpand()
        {
            NavigationService.Navigate<MapPage>(NavigationService.FrameKeyMain);
        }

        private bool CanExpand()
        {
            return !NavigationService.IsPageInFrame<MapPage>(NavigationService.FrameKeyMain);
        }
    }
}
