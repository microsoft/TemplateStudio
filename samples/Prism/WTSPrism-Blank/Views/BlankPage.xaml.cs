using Windows.UI.Xaml.Controls;

using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class BlankPage : Page
    {
        private BlankPageViewModel ViewModel => DataContext as BlankPageViewModel;

        public BlankPage()
        {
            InitializeComponent();
        }
    }
}
