using System;

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

using GalaSoft.MvvmLight;

namespace wts.ItemName.ViewModels
{
    public class ShellNavigationItem : ViewModelBase
    {
        public string Label { get; set; }

        public Symbol Symbol { get; set; }

        public string ViewModelName { get; set; }

        private Visibility _selectedVis = Visibility.Collapsed;

        public Visibility SelectedVis
        {
            get { return _selectedVis; }

            set { Set(ref _selectedVis, value); }
        }

        public char SymbolAsChar
        {
            get { return (char)Symbol; }
        }

        private readonly IconElement _iconElement = null;

        public IconElement Icon
        {
            get
            {
                var foregroundBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("SelectedForeground"),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                };

                if (_iconElement != null)
                {
                    BindingOperations.SetBinding(_iconElement, IconElement.ForegroundProperty, foregroundBinding);

                    return _iconElement;
                }

                var fontIcon = new FontIcon { FontSize = 16, Glyph = SymbolAsChar.ToString() };

                BindingOperations.SetBinding(fontIcon, IconElement.ForegroundProperty, foregroundBinding);

                return fontIcon;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                Set(ref _isSelected, value);

                bool isFcu = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5);
                SelectedVis = isFcu && value ? Visibility.Visible : Visibility.Collapsed;

                SelectedForeground = IsSelected
                    ? Application.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush
                    : GetStandardTextColorBrush();
            }
        }

        private SolidColorBrush _selectedForeground = null;

        public SolidColorBrush SelectedForeground
        {
            get { return _selectedForeground ?? (_selectedForeground = GetStandardTextColorBrush()); }

            set { Set(ref _selectedForeground, value); }
        }

        public ShellNavigationItem(string label, Symbol symbol, string viewModelName)
            : this(label, viewModelName)
        {
            Symbol = symbol;
        }

        public ShellNavigationItem(string label, IconElement icon, string viewModelName)
            : this(label, viewModelName)
        {
            _iconElement = icon;
        }

        public ShellNavigationItem(string label, string viewModelName)
        {
            Label = label;
            ViewModelName = viewModelName;
        }

        private SolidColorBrush GetStandardTextColorBrush()
        {
            var brush = Application.Current.Resources["ThemeControlForegroundBaseHighBrush"] as SolidColorBrush;

            return brush;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
