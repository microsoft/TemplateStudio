using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DragAndDropExample.Views
{
    public sealed partial class Scenario3Page : Page
    {
        public Scenario3ViewModel ViewModel { get; } = new Scenario3ViewModel();

        public Scenario3Page()
        {
            InitializeComponent();
        }
    }
}
