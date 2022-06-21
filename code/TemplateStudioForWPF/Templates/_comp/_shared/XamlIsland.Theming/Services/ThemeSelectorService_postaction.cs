//{[{
using System.Windows.Media;
//}]}

namespace Param_RootNamespace.Services;

public class ThemeSelectorService : IThemeSelectorService
{
//{[{
    public event EventHandler ThemeChanged;

//}]}
    public ThemeSelectorService()
    {
    }
//{[{
    public SolidColorBrush GetColor(string colorKey)
                => Application.Current.FindResource(colorKey) as SolidColorBrush;
//}]}
    public void SetTheme(AppTheme theme)
    {
//^^
//{[{
        ThemeChanged?.Invoke(this, EventArgs.Empty);
//}]}
    }
}
