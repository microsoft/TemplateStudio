using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
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
        // TODO WTS: Set your preferred default zoom level
        private const double DefaultZoomLevel = 17;

        private readonly ILocationService _locationService;

        // TODO WTS: Set your preferred default location if a geolock can't be found.
        private readonly BasicGeoposition _defaultPosition = new BasicGeoposition()
        {
            Latitude = 47.609425,
            Longitude = -122.3417
        };

        private string _mapServiceToken;

        public string MapServiceToken
        {
            get { return _mapServiceToken; }
            set { Param_Setter(ref _mapServiceToken, value); }
        }

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

        private ObservableCollection<MapIcon> _mapIcons = new ObservableCollection<MapIcon>();

        public ObservableCollection<MapIcon> MapIcons
        {
            get { return _mapIcons; }
            set { Param_Setter(ref _mapIcons, value); }
        }

        public MapPageViewModel(ILocationService locationServiceInstance)
        {
            _locationService = locationServiceInstance;
            Center = new Geopoint(_defaultPosition);
            ZoomLevel = DefaultZoomLevel;

            // TODO WTS: Set your map service token. If you don't have one, request from https://www.bingmapsportal.com/
            // MapServiceToken = string.Empty;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
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

            var mapIcon = new MapIcon()
            {
                Location = Center,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = "Map_YourLocation".GetLocalized(),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map.png")),
                ZIndex = 0
            };
            MapIcons.Add(mapIcon);
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
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
