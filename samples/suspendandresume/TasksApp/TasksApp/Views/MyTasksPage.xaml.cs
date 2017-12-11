using TasksApp.Helpers;
using TasksApp.Services;
using TasksApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TasksApp.Views
{
    public sealed partial class MyTasksPage : Page
    {
        public static MyTasksPage Current;
        public MyTasksViewModel ViewModel { get; } = new MyTasksViewModel();
        public MyTasksPage()
        {
            Current = this;
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadDataAsync(WindowStates.CurrentState, e.Parameter as SuspensionState);
            Singleton<SuspendAndResumeService>.Instance.OnBackgroundEntering += Instance_OnBackgroundEntering;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Singleton<SuspendAndResumeService>.Instance.OnBackgroundEntering -= Instance_OnBackgroundEntering;
        }

        private void Instance_OnBackgroundEntering(object sender, OnBackgroundEnteringEventArgs e)
        {
            ViewModel.SaveData(ref e);
        }
    }
}
