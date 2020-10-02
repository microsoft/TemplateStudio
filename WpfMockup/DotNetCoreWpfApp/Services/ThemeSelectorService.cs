using System;
using System.Windows;
using System.Windows.Media;
using ControlzEx.Theming;

using DotNetCoreWpfApp.Contracts.Services;
using DotNetCoreWpfApp.Models;

using Microsoft.Win32;

namespace DotNetCoreWpfApp.Services
{
    public class ThemeSelectorService : IThemeSelectorService
    {
        public event EventHandler ThemeChanged;

        private bool IsHighContrastActive
                        => SystemParameters.HighContrast;

        public ThemeSelectorService()
        {
            SystemEvents.UserPreferenceChanging += OnUserPreferenceChanging;
        }

        public SolidColorBrush GetColor(string colorKey)
                    => Application.Current.FindResource(colorKey) as SolidColorBrush;

        public bool SetTheme(AppTheme? theme = null)
        {
            if (IsHighContrastActive)
            {
                // TODO WTS: Set high contrast theme
                // You can add custom themes following the docs on https://mahapps.com/docs/themes/thememanager
            }
            else if (theme == null)
            {
                if (App.Current.Properties.Contains("Theme"))
                {
                    // Read saved theme from properties
                    var themeName = App.Current.Properties["Theme"].ToString();
                    theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
                }
                else
                {
                    // Set default theme
                    theme = AppTheme.Light;
                }
            }

            var currentTheme = ThemeManager.Current.DetectTheme(Application.Current);
            if (currentTheme == null || currentTheme.Name != theme.ToString())
            {
                ThemeManager.Current.ChangeTheme(Application.Current, $"{theme}.Blue");
                App.Current.Properties["Theme"] = theme.ToString();
                ThemeChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        public AppTheme GetCurrentTheme()
        {
            var themeName = App.Current.Properties["Theme"]?.ToString();
            Enum.TryParse(themeName, out AppTheme theme);
            return theme;
        }

        private void OnUserPreferenceChanging(object sender, UserPreferenceChangingEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Color ||
                e.Category == UserPreferenceCategory.VisualStyle)
            {
                SetTheme();
            }
        }
    }
}
