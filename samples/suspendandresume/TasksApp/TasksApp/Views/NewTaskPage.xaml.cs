using TasksApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TasksApp.Views
{
    public sealed partial class NewTaskPage : Page
    {
        public NewTaskViewModel ViewModel { get; } = new NewTaskViewModel();
        public NewTaskPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(title);
        }
    }
}
