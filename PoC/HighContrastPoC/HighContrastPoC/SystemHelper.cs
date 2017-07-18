using System.ComponentModel;
using System.Windows;

namespace HighContrastPoC.Helpers
{
    public class SystemHelper : DependencyObject
    {
        private SystemHelper()
        {
            SystemParameters.StaticPropertyChanged += OnStaticPropertyChanged;
        }

        private static SystemHelper _instance;
        public static SystemHelper Instance => _instance ?? (_instance = new SystemHelper());         

        void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HighContrast")
            {
                Instance.IsHighContrast = SystemParameters.HighContrast;
            }
        }        

        public static readonly DependencyProperty IsHighContrastProperty = DependencyProperty.Register("IsHighContrast", typeof(bool), typeof(SystemHelper), new PropertyMetadata(false));
        public bool IsHighContrast
        {
            get => (bool)GetValue(IsHighContrastProperty);
            private set => SetValue(IsHighContrastProperty, value);
        }
    }
}
