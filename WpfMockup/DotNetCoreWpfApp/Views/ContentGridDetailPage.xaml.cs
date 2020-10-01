using System.Windows.Controls;

using DotNetCoreWpfApp.ViewModels;

namespace DotNetCoreWpfApp.Views
{
    public partial class ContentGridDetailPage : Page
    {
        public ContentGridDetailPage(ContentGridDetailViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
