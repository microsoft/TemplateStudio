using System.Windows.Controls;

namespace Param_RootNamespace.Views
{
    public partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public wts.ItemNamePage()
        {
            InitializeComponent();
        }
    }
}
