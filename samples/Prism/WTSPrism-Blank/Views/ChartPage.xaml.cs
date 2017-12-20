using Windows.UI.Xaml.Controls;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class ChartPage : Page
    {
        private ChartPageViewModel ViewModel => DataContext as ChartPageViewModel;

        // TODO: UWPTemplates: Change the chart as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        public ChartPage()
        {
            InitializeComponent();
        }
    }
}
