using uct.SplitViewProject.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uct.SplitViewProject.View
{
    public sealed partial class ShellView : Page
    {
        public ShellView()
        {
            DataContext = ViewModel;
            this.InitializeComponent();

            ViewModel.Initialize(frame);
        }
    }
}
