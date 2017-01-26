using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace BasicSplitViewProject.Shell
{
    public class ShellNavigationItem
    {
        public string Glyph { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }

        private ShellNavigationItem(string resource, string glyph, Type pageType)
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            this.Name = resourceLoader.GetString(resource);
            this.Glyph = glyph;
            this.PageType = pageType;
        }

        public static ShellNavigationItem FromType<T>(string resource, string glyph) where T : Page
        {
            return new ShellNavigationItem(resource, glyph, typeof(T));
        }
    }
}