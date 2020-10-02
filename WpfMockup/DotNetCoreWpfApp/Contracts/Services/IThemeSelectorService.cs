using System;
using System.Windows.Media;
using DotNetCoreWpfApp.Models;

namespace DotNetCoreWpfApp.Contracts.Services
{
    public interface IThemeSelectorService
    {
        event EventHandler ThemeChanged;

        SolidColorBrush GetColor(string colorKey);

        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
