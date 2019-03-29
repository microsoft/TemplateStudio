using System;
using Windows.Devices.Geolocation;

namespace Param_RootNamespace.Views
{
    public interface IMapPageView
    {
        void AddMapIcon(Geopoint position, string title);
    }
}
