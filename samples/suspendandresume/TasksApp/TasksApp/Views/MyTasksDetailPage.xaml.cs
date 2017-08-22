using TasksApp.Models;
using TasksApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TasksApp.Views
{
    public sealed partial class MyTasksDetailPage : Page
    {
        public MyTasksDetailViewModel ViewModel { get; } = new MyTasksDetailViewModel();
        public MyTasksDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Item = e.Parameter as TaskModel;
        }
    }
}
