using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ThemesApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            LoadTheme(SystemParameters.HighContrast);
        }

        private void LoadTheme(bool highContrast)
        {
            Uri themeUri;
            if (highContrast)
            {
                themeUri = new Uri("/ThemesApp;component/Styles/HighContrastTheme.xaml", UriKind.RelativeOrAbsolute);
            }
            else
            {
                themeUri = new Uri("/ThemesApp;component/Styles/DefaultTheme.xaml", UriKind.RelativeOrAbsolute);
            }
            Current.Resources.MergedDictionaries.Clear();
            Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = themeUri
            });
        }
    }
}
