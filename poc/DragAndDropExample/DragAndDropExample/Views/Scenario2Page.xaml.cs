using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DragAndDropExample.Views
{
    public sealed partial class Scenario2Page : Page
    {
        public Scenario2ViewModel ViewModel { get; } = new Scenario2ViewModel();

        public Scenario2Page()
        {
            InitializeComponent();
        }
    }
}
