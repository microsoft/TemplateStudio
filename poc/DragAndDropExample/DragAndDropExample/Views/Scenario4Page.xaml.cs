using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DragAndDropExample.Views
{
    public sealed partial class Scenario4Page : Page
    {
        public Scenario4ViewModel ViewModel { get; } = new Scenario4ViewModel();

        public Scenario4Page()
        {
            InitializeComponent();
        }
    }
}
