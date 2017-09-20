using Windows.ApplicationModel.Resources;

namespace Param_ItemNamespace
{
    public static class StringExtensions
    {
        public static string GetResourceString(this string resourceKey)
        {
            var resourceLoader = new ResourceLoader();
            return resourceLoader.GetString(resourceKey);
        }
    }
}
