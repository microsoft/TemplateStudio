using Windows.UI.Xaml.Controls;

namespace uct.SplitViewProject.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            InitializeComponent();

            ViewModel.Initialize(shellFrame, primaryListView, secondaryListView);
        }
    }
}
