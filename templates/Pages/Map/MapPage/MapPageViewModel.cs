using System;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;

namespace ItemNamespace.MapPage
{
    public class MapPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private MapControl _map;

        private double _zoomLevel;
        public double ZoomLevel
        {
            get => _zoomLevel;
            set => Set(ref _zoomLevel, value);
        }

        private Geopoint _center;
        public Geopoint Center
        {
            get => _center;
            set => Set(ref _center, value);
        }

        public ICommand LoadDataCommand { get; private set; }

        public MapPageViewModel()
        {
            LoadDataCommand = new RelayCommand(LoadDataAsync);
            ZoomLevel = 19;
        }

        public void SetMap(MapControl map) => _map = map;

        private void LoadDataAsync()
        {
            var position = new BasicGeoposition() { Latitude = 47.639627, Longitude = -122.128227 };
            Center = new Geopoint(position);
            AddMapIcon(Center, "Microsoft Corporation");
        }

        private void AddMapIcon(Geopoint position, string title)
        {
            MapIcon mapIcon = new MapIcon();
            mapIcon.Location = position;
            mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon.Title = title;
            mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///MapPage/map.png"));
            mapIcon.ZIndex = 0;
            _map?.MapElements.Add(mapIcon);
        }
    }
}