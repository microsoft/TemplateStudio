using System;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;

using Windows.Storage;
using Windows.UI.Core;

using WinUIDesktopApp.Contracts.Services;
using WinUIDesktopApp.Helpers;

namespace WinUIDesktopApp.Services
{
    public class ThemeSelectorService : IThemeSelectorService
    {
        private const string SettingsKey = "AppBackgroundRequestedTheme";

        public ElementTheme Theme { get; set; } = ElementTheme.Default;

        public async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();
            await Task.CompletedTask;
        }

        public async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            await SetRequestedThemeAsync();
            await SaveThemeInSettingsAsync(Theme);
        }

        public async Task SetRequestedThemeAsync()
        {
            if (App.MainWindow.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = Theme;
            }

            await Task.CompletedTask;
        }

        private async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
        }
    }
}
