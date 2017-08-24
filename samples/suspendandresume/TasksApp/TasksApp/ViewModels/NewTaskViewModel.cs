using System;
using System.Collections.Generic;

using TasksApp.Helpers;
using TasksApp.Models;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TasksApp.ViewModels
{
    public class NewTaskViewModel : Observable
    {
        private TextBox _titleTextBox;
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                Set(ref _title, value);
                SaveCommand.OnCanExecuteChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(OnSave, CanSave));

        private bool CanSave() => !string.IsNullOrEmpty(Title);

        private async void OnSave()
        {
            var data = await ApplicationData.Current.LocalFolder.ReadAsync<List<TaskModel>>("MyTasks");
            data.Add(new TaskModel()
            {
                Title = Title,
                Description = Description
            });
            await ApplicationData.Current.LocalFolder.SaveAsync("MyTasks", data);
            Title = string.Empty;
            Description = string.Empty;
            _titleTextBox.Focus(FocusState.Keyboard);
        }

        public NewTaskViewModel()
        {
        }

        public void Initialize(TextBox titleTextBox)
        {
            _titleTextBox = titleTextBox;
        }
    }
}
