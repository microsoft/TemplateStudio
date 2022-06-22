//{[{
using System.Windows.Media;
//}]}

namespace Param_RootNamespace.Contracts.Services;

public interface IThemeSelectorService
{
//{[{
    event EventHandler ThemeChanged;

    SolidColorBrush GetColor(string colorKey);

//}]}
}

