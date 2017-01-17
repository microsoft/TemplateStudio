using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace SplitViewTemplate.Model
{
    public class ShellNavigationItem
    {
        public string UID { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string PageName { get; set; }
        public string Glyph { get; set; }
        public Visibility IconVisibility { get; set; }

        static public ShellNavigationItem FromIcon(string icon, string pageName, string uid = null, string name = null)
        {
            var item = new ShellNavigationItem()
            {
                Icon = icon,
                Name = name,
                PageName = pageName,
                IconVisibility = Visibility.Visible
            };
            string resource = GetFromUID(uid);
            if (!String.IsNullOrEmpty(resource))
            {
                item.Name = resource;
            }
            return item;
        }

        static public ShellNavigationItem FromGlyph(string glyph, string pageName, string uid = null, string name = null)
        {
            var item = new ShellNavigationItem()
            {
                Glyph = glyph,
                Name = name,
                PageName = pageName,
                IconVisibility = Visibility.Collapsed,
                Icon = "Assets/Icons/Home.png"
            };
            string resource = GetFromUID(uid);
            if (!String.IsNullOrEmpty(resource))
            {
                item.Name = resource;
            }
            return item;
        }

        static private string GetFromUID(string uid)
        {
            if (!String.IsNullOrEmpty(uid))
            {
                var resourceLoader = new ResourceLoader();
                var resource = resourceLoader.GetString(uid);
                if (!String.IsNullOrEmpty(resource))
                {
                    return resource;
                }
            }
            return String.Empty;
        }
    }
}
