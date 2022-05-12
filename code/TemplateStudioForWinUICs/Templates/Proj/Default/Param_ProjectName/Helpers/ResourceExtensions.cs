using Microsoft.Windows.ApplicationModel.Resources;

namespace Param_RootNamespace.Helpers
{
    internal static class ResourceExtensions
    {
        private static readonly ResourceLoader _resourceLoader = new();

        public static string GetLocalized(this string resourceKey)
        {
            return _resourceLoader.GetString(resourceKey);
        }
    }
}
