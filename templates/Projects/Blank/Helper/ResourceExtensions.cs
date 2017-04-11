using System.Runtime.InteropServices;
using Windows.ApplicationModel.Resources;

namespace uct.BlankProject.Helper
{
    internal static class ResourceExtensions
    {
        public static string GetLocalized(this string resourceKey)
        {
            return new ResourceLoader().GetString(resourceKey);
        }
    }
}
