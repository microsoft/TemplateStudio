using Microsoft.Templates.UI.ViewModels.NewItem;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class ChangesSummaryView : Page
    {
        public MainViewModel ViewModel { get; }

        public ChangesSummaryView()
        {
            ViewModel = MainViewModel.Current;
            DataContext = ViewModel;
            InitializeComponent();

            Loaded += (sender, args) => { ViewModel.ChangesSummary.Initialize(); };
        }
    }
}
