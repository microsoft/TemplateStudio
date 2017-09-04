using System.Windows.Input;
using TasksApp.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TasksApp.Views
{
    public sealed partial class MyTasksDetailControl : UserControl
    {
        public TaskModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as TaskModel; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem",typeof(TaskModel),typeof(MyTasksDetailControl),new PropertyMetadata(null));

        public ICommand DeleteCommand
        {
            get { return GetValue(DeleteCommandProperty) as ICommand; }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static DependencyProperty DeleteCommandProperty = DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(MyTasksDetailControl), new PropertyMetadata(null));

        public MyTasksDetailControl()
        {
            InitializeComponent();
            DeleteCommand = MyTasksPage.Current.ViewModel.DeleteCommand;
        }
    }
}
