using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DotNetCoreWpfApp.XamlIsland
{
    public sealed partial class XAMLIslandControlUniversal : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public bool UseDarkTheme
        {
            get { return (bool)GetValue(UseDarkThemeProperty); }
            set { SetValue(UseDarkThemeProperty, value); }
        }

        // TODO WTS: Add any Dependency properties you need to add to your UWP control
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(XAMLIslandControlUniversal), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(XAMLIslandControlUniversal), new PropertyMetadata(null));

        public static readonly DependencyProperty UseDarkThemeProperty = DependencyProperty.Register(nameof(UseDarkTheme), typeof(bool), typeof(XAMLIslandControlUniversal), new PropertyMetadata(false, OnUseDarkThemePropertyChanged));

        public XAMLIslandControlUniversal()
        {
            InitializeComponent();
        }

        private static void OnUseDarkThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is XAMLIslandControlUniversal control && e.NewValue is bool useDarkTheme)
            {
                control.RequestedTheme = useDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
            }
        }
    }
}
