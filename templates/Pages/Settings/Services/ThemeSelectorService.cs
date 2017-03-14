using System;
using Windows.Storage;
using Windows.UI.Xaml;
using RootNamespace.Extensions;

namespace RootNamespace.Services
{
    public enum Theme
    {
        Light,
        Dark
    }

    public static class ThemeSelectorService
    {
        private const string SettingsKey = "RequestedTheme";

        public static bool IsLightThemeEnabled => Theme == Theme.Light;
        public static Theme Theme { get; set; }

        static ThemeSelectorService()
        {
            Theme = LoadThemeFromSettings();
        }

        public static void SwitchTheme()
        {
            if (Theme == Theme.Dark)
            {
                SetTheme(Theme.Light);
            }
            else
            {
                SetTheme(Theme.Dark);
            }
        }
        public static void SetTheme(Theme theme)
        {
            Theme = theme;
            SetRequestedTheme();
            SaveThemeInSettings(Theme);
        }
        public static void SetRequestedTheme()
        {
            var elementTheme = Theme == Theme.Dark ? ElementTheme.Dark : ElementTheme.Light;
            var frameworkElement = Window.Current.Content as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.RequestedTheme = elementTheme;
            }
        }

        private static Theme LoadThemeFromSettings()
        {
            Theme cacheTheme = Theme.Light;
            string themeName = ApplicationData.Current.LocalSettings.Read<string>(SettingsKey);
            if (String.IsNullOrEmpty(themeName))
            {
                cacheTheme = Application.Current.RequestedTheme == ApplicationTheme.Dark ? Theme.Dark : Theme.Light;
            }
            else
            {
                Enum.TryParse<Theme>(themeName, out cacheTheme);
            }
            return cacheTheme;
        }
        private static void SaveThemeInSettings(Theme theme)
        {
            ApplicationData.Current.LocalSettings.Save<string>(SettingsKey, theme.ToString());
        }
    }
}