using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace InkPoc.Controls
{
    internal static class InkOptionHelper
    {
        internal static AppBarButton BuildAppBarButton(string label, string codeString)
        {
            int code = int.Parse(codeString, NumberStyles.HexNumber);
            var icon = new FontIcon()
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Glyph = char.ConvertFromUtf32(code)
            };
            return BuildAppBarButton(label, icon);
        }

        internal static AppBarButton BuildAppBarButton(string label, Symbol icon)
        {
            return BuildAppBarButton(label, new SymbolIcon(icon));
        }

        internal static InkToolbarCustomToolButton BuildInkToolbarCustomToolButton(string label, string codeString)
        {
            int code = int.Parse(codeString, NumberStyles.HexNumber);
            var icon = new FontIcon()
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Glyph = char.ConvertFromUtf32(code)
            };
            return BuildInkToolbarCustomToolButton(label, icon);
        }

        internal static InkToolbarCustomToggleButton BuildInkToolbarCustomToggleButton(string label, string codeString)
        {
            int code = int.Parse(codeString, NumberStyles.HexNumber);
            var icon = new FontIcon()
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Glyph = char.ConvertFromUtf32(code)
            };
            return BuildInkToolbarCustomToggleButton(label, icon);
        }

        internal static InkToolbarCustomToolButton BuildInkToolbarCustomToolButton(string label, Symbol icon)
        {
            return BuildInkToolbarCustomToolButton(label, new SymbolIcon(icon));
        }

        internal static InkToolbarCustomToggleButton BuildInkToolbarCustomToggleButton(string label, Symbol icon)
        {
            return BuildInkToolbarCustomToggleButton(label, new SymbolIcon(icon));
        }

        internal static AppBarButton BuildAppBarButton(string label, IconElement icon)
        {
            var appBarButton = new AppBarButton()
            {
                Label = label,
                Icon = icon,
                BorderThickness = new Thickness(0, 0, 0, 0)
            };
            ToolTipService.SetToolTip(appBarButton, label);
            return appBarButton;
        }

        internal static InkToolbarCustomToolButton BuildInkToolbarCustomToolButton(string label, IconElement icon)
        {
            var inkToolbarCustomToolButton = new InkToolbarCustomToolButton()
            {
                Content = icon
            };
            ToolTipService.SetToolTip(inkToolbarCustomToolButton, label);
            return inkToolbarCustomToolButton;
        }

        internal static InkToolbarCustomToggleButton BuildInkToolbarCustomToggleButton(string label, IconElement icon)
        {
            var inkToolbarCustomToggleButton = new InkToolbarCustomToggleButton()
            {
                Content = icon
            };
            ToolTipService.SetToolTip(inkToolbarCustomToggleButton, label);
            return inkToolbarCustomToggleButton;
        }
    }
}
