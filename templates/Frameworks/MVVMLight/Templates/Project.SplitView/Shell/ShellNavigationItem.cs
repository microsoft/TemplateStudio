using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace ItemName.Shell
{
    public class ShellNavigationItem
    {
        public string Glyph { get; set; }
        public string Name { get; set; }        
        public string ViewModelName { get; set; }

        public ShellNavigationItem(string resource, string glyph, string viewModelName)
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            this.Name = resourceLoader.GetString(resource);
            this.Glyph = glyph;
            this.ViewModelName = viewModelName;
        }      
    }
}