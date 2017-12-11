using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;

using WTSGeneratedPivot.Helpers;
using WTSGeneratedPivot.Services;

namespace WTSGeneratedPivot.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
         // TODO WTS: Set your preferred default zoom level
        private const double DefaultZoomLevel = 17;

        private readonly ILocationService locationService;

        // TODO WTS: Set your preferred default location if a geolock can't be found.
        private readonly BasicGeoposition defaultPosition = new BasicGeoposition()
        {
            Latitude = 47.609425,
            Longitude = -122.3417
        };

        private string mapServiceToken;

        public string MapServiceToken
        {
            get { return mapServiceToken; }
            set { SetProperty(ref mapServiceToken, value); }
        }

        private double zoomLevel;

        public double ZoomLevel
        {
            get { return zoomLevel; }
            set { SetProperty(ref zoomLevel, value); }
        }

        private Geopoint center;

        public Geopoint Center
        {
            get { return center; }
            set { SetProperty(ref center, value); }
        }

        private ObservableCollection<MapIcon> mapIcons = new ObservableCollection<MapIcon>();

        public ObservableCollection<MapIcon> MapIcons
        {
            get { return mapIcons; }
            set { SetProperty(ref mapIcons, value); }
        }

        public MapViewModel(ILocationService locationServiceInstance)
        {
            locationService = locationServiceInstance;
            Center = new Geopoint(defaultPosition);
            ZoomLevel = DefaultZoomLevel;

            // TODO WTS: Set your map service token. If you don't have it, request at https://www.bingmapsportal.com/
            MapServiceToken = string.Empty;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
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
   }
}
