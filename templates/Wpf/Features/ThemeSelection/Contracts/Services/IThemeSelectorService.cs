using System;
using System.Windows.Media;
using Param_RootNamespace.Models;

namespace Param_RootNamespace.Contracts.Services
{
    public interface IThemeSelectorService
    {
        event EventHandler ThemeChanged;

        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();

        SolidColorBrush GetColor(string colorKey);
    }
}
