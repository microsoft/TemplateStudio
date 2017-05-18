using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Services;
using System.Threading.Tasks;

namespace Param_ItemNamespace.Views
{
    public sealed partial class MapPagePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
// FRAGMENT _fragments\Page_Map_Properties.fragment.cs

        public MapPagePage()
        {
            locationService = new LocationService();
            Center = new Geopoint(defaultPosition);
            ZoomLevel = defaultZoomLevel;
            InitializeComponent();
        }

        public async Task InitializeAsync()
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

            if (mapControl != null)
            {
                // TODO UWPTemplates: Set your map service token. If you don't have it, request at https://www.bingmapsportal.com/            
                mapControl.MapServiceToken = "";

                AddMapIcon(Center, "Map_YourLocation".GetLocalized());
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

        private void AddMapIcon(Geopoint position, string title)
        {
            MapIcon mapIcon = new MapIcon()
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
