using System;

using Windows.UI.Xaml.Controls;

namespace uct.ItemName.Shell
{
    public class ShellNavigationItem
    {
        public string Glyph { get; set; }
        public string Name { get; set; }        
        public Type PageType { get; set; }

        private ShellNavigationItem(string name, string glyph, Type pageType)
        {
            this.Name = name;
            this.Glyph = glyph;
            this.PageType = pageType;
        }

        public static ShellNavigationItem FromType<T>(string name, string glyph) where T : Page
        {
            return new ShellNavigationItem(name, glyph, typeof(T));
        }       
    }
}