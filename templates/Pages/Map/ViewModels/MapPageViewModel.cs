using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class MapPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
// FRAGMENT _fragments\Page_Map_Properties.fragment.cs

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
                // TODO UWPTemplates: Set your map service token. If you don't have it, request at https://www.bingmapsportal.com/            
                map.MapServiceToken = "";

                AddMapIcon(map, Center, "Map_YourLocation".GetLocalized());
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
