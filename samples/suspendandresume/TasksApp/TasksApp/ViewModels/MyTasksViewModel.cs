using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using TasksApp.Helpers;
using TasksApp.Models;
using TasksApp.Services;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TasksApp.ViewModels
{
    public class MyTasksViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState _currentState;

        private TaskModel _selected;
        public TaskModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private Visibility _hasTasksVisibility = Visibility.Collapsed;
        public Visibility HasTasksVisibility
        {
            get { return _hasTasksVisibility; }
            set { Set(ref _hasTasksVisibility, value); }
        }

        private Visibility _hasNoTasksVisibility = Visibility.Visible;
        public Visibility HasNoTasksVisibility
        {
            get { return _hasNoTasksVisibility; }
            set { Set(ref _hasNoTasksVisibility, value); }
        }

        public ICommand ItemClickCommand { get; private set; }
        public ICommand StateChangedCommand { get; private set; }        
        public RelayCommand DeleteCommand { get; private set; }

        public ObservableCollection<TaskModel> Tasks { get; private set; } = new ObservableCollection<TaskModel>();

        public MyTasksViewModel()
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
            DeleteCommand = new RelayCommand(OnDelete);
        }

        public async Task LoadDataAsync(VisualState currentState, SuspensionState suspensionState)
        {
            _currentState = currentState;
            Tasks.Clear();
            var data = await ApplicationData.Current.LocalFolder.ReadAsync<IEnumerable<TaskModel>>("MyTasks");
            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    Tasks.Add(item);
                    HasTasksVisibility = Visibility.Visible;
                    HasNoTasksVisibility = Visibility.Collapsed;
                }
                if (suspensionState != null && suspensionState.Data != null)
                {
                    int index = int.Parse(suspensionState.Data.ToString());
                    Selected = Tasks.ElementAt(index);
                }
                else
                {
                    Selected = Tasks.First();
                }
            }                        
        }

        public void SaveData(ref OnBackgroundEnteringEventArgs e)
        {
            e.SuspensionState.Data = Tasks.IndexOf(Selected);
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            TaskModel item = args?.ClickedItem as TaskModel;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate<Views.MyTasksDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
            }
        }        

        private async void OnDelete()
        {
            var oldIndex = Tasks.IndexOf(Selected);
            Tasks.Remove(Selected);
            if (!Tasks.Any())
            {
                HasTasksVisibility = Visibility.Collapsed;
                HasNoTasksVisibility = Visibility.Visible;
            }            
            if (Tasks.Count == 1)
            {
                Selected = Tasks.First();
            }
            else if(Tasks.Count > 1)
            {
                int newIndex = oldIndex;
                if (newIndex > 0)
                {
                    newIndex--;
                }
                Selected = Tasks.ElementAt(newIndex);
            }
            await ApplicationData.Current.LocalFolder.SaveAsync("MyTasks", Tasks);
        }
    }
}
