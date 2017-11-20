using System;
using Windows.Devices.Geolocation;

namespace Param_ItemNamespace.Views
{
    public interface IMapPageView
    {
        void AddMapIcon(Geopoint position, string title);
    }
}
