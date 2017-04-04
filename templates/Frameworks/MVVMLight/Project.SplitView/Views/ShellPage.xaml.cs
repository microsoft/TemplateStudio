using uct.ItemName.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using uct.ItemName.ViewModels;

namespace uct.ItemName.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel { get { return DataContext as ShellViewModel; } }
        
        public ShellPage()
        {
            this.InitializeComponent();

            ViewModel.Initialize(shellFrame, primaryListView, secondaryListView);
        }
    }
}
