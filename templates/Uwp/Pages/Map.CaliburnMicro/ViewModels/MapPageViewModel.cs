using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
using Param_RootNamespace.Views;

namespace Param_RootNamespace.ViewModels
{
    public class MapPageViewModel : System.ComponentModel.INotifyPropertyChanged
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
            set { Param_Setter(ref _zoomLevel, value); }
        }

        private Geopoint _center;

        public Geopoint Center
        {
            get { return _center; }
            set { Param_Setter(ref _center, value); }
        }

        private string _mapServiceToken;

        public string MapServiceToken
        {
            get { return _mapServiceToken; }
            set { Param_Setter(ref _mapServiceToken, value); }
        }

        public MapPageViewModel()
        {
            _locationService = new LocationService();
            Center = new Geopoint(_defaultPosition);
            ZoomLevel = DefaultZoomLevel;
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            if (_locationService != null)
            {
                _locationService.PositionChanged += LocationServicePositionChanged;

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

            // TODO WTS: Set your map service token. If you don't have one, request from https://www.bingmapsportal.com/
            // MapServiceToken = string.Empty;
            var view = GetView() as IMapPageView;

            view?.AddMapIcon(Center, "Map_YourLocation".GetLocalized());
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationServicePositionChanged;
                _locationService.StopListening();
            }
        }

        private void LocationServicePositionChanged(object sender, Geoposition geoposition)
        {
            if (geoposition != null)
            {
                Center = geoposition.Coordinate.Point;
            }
        }
    }
}
