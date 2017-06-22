using GalaSoft.MvvmLight;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace wts.ItemName.ViewModels
{
    public class ShellNavigationItem : ViewModelBase
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
            get
            {
                return _selectedForeground ?? (_selectedForeground = GetStandardTextColorBrush());
            }
            set { Set(ref _selectedForeground, value); }
        }

        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar { get { return (char)Symbol; } }
        public string ViewModelName { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
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
            var brush = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;

            return brush;
        }

        public ShellNavigationItem(string label, Symbol symbol, string viewModelName)
        {
            this.Label = label;
            this.Symbol = symbol;
            this.ViewModelName = viewModelName;
        }
    }
}
