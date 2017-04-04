using uct.ItemName.Services;
using uct.ItemName.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uct.ItemName.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();
        
        public ShellPage()
        {
            this.InitializeComponent();

            ViewModel.Initialize(shellFrame, primaryListView, secondaryListView);
        }
    }
}
