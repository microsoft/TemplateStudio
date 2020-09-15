//{[{
using System.Windows.Media;
//}]}

namespace Param_RootNamespace.Services
{
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
        public bool SetTheme(AppTheme? theme = null)
        {
            if (currentTheme == null || currentTheme.Name != theme.ToString())
            {
//^^
//{[{
                ThemeChanged?.Invoke(this, EventArgs.Empty);
//}]}
                return true;
            }
        }
    }
}
