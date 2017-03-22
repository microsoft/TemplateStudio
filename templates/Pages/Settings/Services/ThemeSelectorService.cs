using System;
using Windows.Storage;
using Windows.UI.Xaml;
using RootNamespace.Extensions;

namespace RootNamespace.Services
{
    public static class ThemeSelectorService
    {
        private const string SettingsKey = "RequestedTheme";

        public static bool IsLightThemeEnabled => Theme == ElementTheme.Light;
        public static ElementTheme Theme { get; set; }

        static ThemeSelectorService()
        {
            Theme = LoadThemeFromSettings();
        }

        public static void SwitchTheme()
        {
            if (Theme == ElementTheme.Dark)
            {
                SetTheme(ElementTheme.Light);
            }
            else
            {
                SetTheme(ElementTheme.Dark);
            }
        }
        
        public static void SetTheme(ElementTheme theme)
        {
            Theme = theme;
            SetRequestedTheme();
            SaveThemeInSettings(Theme);
        }

        public static void SetRequestedTheme()
        {
            var frameworkElement = Window.Current.Content as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.RequestedTheme = Theme;
            }
        }

        private static ElementTheme LoadThemeFromSettings()
        {
            ElementTheme cacheTheme = ElementTheme.Light;
            string themeName = ApplicationData.Current.LocalSettings.Read<string>(SettingsKey);
            if (String.IsNullOrEmpty(themeName))
            {
                cacheTheme = Application.Current.RequestedTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
            }
            else
            {
                Enum.TryParse<ElementTheme>(themeName, out cacheTheme);
            }
            return cacheTheme;
        }

        private static void SaveThemeInSettings(ElementTheme theme)
        {
            ApplicationData.Current.LocalSettings.Save<string>(SettingsKey, theme.ToString());
        }
    }
}