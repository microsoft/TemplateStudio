using MarkdownViewer.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MarkdownViewer.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        public MainPage()
        {
            InitializeComponent();
        }
    }
}
