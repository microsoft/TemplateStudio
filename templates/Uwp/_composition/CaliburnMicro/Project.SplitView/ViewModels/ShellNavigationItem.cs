using System;
using Caliburn.Micro;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Param_ItemNamespace.Helpers;

namespace wts.ItemName.ViewModels
{
    public class ShellNavigationItem : PropertyChangedBase
    {
        private bool _isSelected;

        private Visibility _selectedVis = Visibility.Collapsed;

        public Visibility SelectedVis
        {
            get { return _selectedVis; }
            set { Set(ref _selectedVis, value); }
        }

        private SolidColorBrush _selectedForeground = null;

        public SolidColorBrush SelectedForeground
        {
            get { return _selectedForeground ?? (_selectedForeground = GetStandardTextColorBrush()); }
            set { Set(ref _selectedForeground, value); }
        }

        public string Label { get; set; }

        public Symbol Symbol { get; set; }

        public char SymbolAsChar
        {
            get { return (char)Symbol; }
        }

        public Type ViewModelType { get; set; }

        private IconElement _iconElement = null;

        public IconElement Icon
        {
            get
            {
                var foregroundBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("SelectedForeground"),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                if (_iconElement != null)
                {
                    BindingOperations.SetBinding(_iconElement, IconElement.ForegroundProperty, foregroundBinding);

                    return _iconElement;
                }

                var fontIcon = new FontIcon { FontSize = 16, Glyph = SymbolAsChar.ToString() };

                BindingOperations.SetBinding(fontIcon, FontIcon.ForegroundProperty, foregroundBinding);

                return fontIcon;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                Set(ref _isSelected, value);
                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
                SelectedForeground = value
                    ? Application.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush
                    : GetStandardTextColorBrush();
            }
        }

        private SolidColorBrush GetStandardTextColorBrush()
        {
            var brush = Application.Current.Resources["ThemeControlForegroundBaseHighBrush"] as SolidColorBrush;

            return brush;
        }

        protected ShellNavigationItem()
        {
        }

        public ShellNavigationItem(string label, Symbol symbol, Type viewModeType)
            : this()
        {
            Label = label;
            Symbol = symbol;
            ViewModelType = viewModeType;
        }

        public ShellNavigationItem(string label, IconElement icon, Type viewModeType)
            : this()
        {
            Label = label;
            _iconElement = icon;
            ViewModelType = viewModeType;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
