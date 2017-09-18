using System;
using System.Windows.Input;

using TasksApp.Helpers;
using TasksApp.Models;
using TasksApp.Services;

using Windows.UI.Xaml;

namespace TasksApp.ViewModels
{
    public class MyTasksDetailViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        public ICommand StateChangedCommand { get; private set; }

        private TaskModel _item;
        public TaskModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public MyTasksDetailViewModel()
        {
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }
        
        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }
    }
}
