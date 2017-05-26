using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Param_RootNamespace.Helpers;
using System.Threading.Tasks;

namespace Param_RootNamespace.Services
{
    public static class ThemeSelectorService
    {
        private const string SettingsKey = "RequestedTheme";

        public static event EventHandler<ElementTheme> OnThemeChanged = delegate { };

        public static bool IsLightThemeEnabled => Theme == ElementTheme.Light;
        public static bool IsDarkThemeEnabled => Theme == ElementTheme.Dark;
        
        public static ElementTheme Theme { get; set; }

        public static async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();
        }

        public static async Task SetThemeAsync(int themeId)
        {
            ElementTheme theme;
            switch (themeId)
            {
                case 0:
                    theme = ElementTheme.Light;
                    break;
                case 1:
                    theme = ElementTheme.Dark;
                    break;
                case 2:
                default:
                    theme = ElementTheme.Default;
                    break;
            }

            await SetThemeAsync(theme);
        }

        public static int GetTheme()
        {
            switch (Theme)
            {
                case ElementTheme.Light:
                    return 0;
                case ElementTheme.Dark:
                    return 1;
                case ElementTheme.Default:
                default:
                    return 2;
            }
        }

        public static async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;
            SetRequestedTheme();
            await SaveThemeInSettingsAsync(Theme);
            OnThemeChanged(null, Theme);
        }

        public static void SetRequestedTheme()
        {
            var frameworkElement = Window.Current.Content as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.RequestedTheme = Theme;
            }
        }

        private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);
            if (!String.IsNullOrEmpty(themeName))
            {
                Enum.TryParse<ElementTheme>(themeName, out cacheTheme);
            }
            return cacheTheme;
        }

        private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync<string>(SettingsKey, theme.ToString());
        }
    }
}
