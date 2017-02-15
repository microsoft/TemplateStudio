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

        public ShellNavigationItem(string name, string glyph, string viewModelName)
        {
            this.Name = name;
            this.Glyph = glyph;
            this.ViewModelName = viewModelName;
        }      
    }
}