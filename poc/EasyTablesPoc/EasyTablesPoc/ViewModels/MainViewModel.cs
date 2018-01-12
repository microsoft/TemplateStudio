using EasyTablesPoc.Helpers;
using EasyTablesPoc.Models;
using EasyTablesPoc.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace EasyTablesPoc.ViewModels
{
    public class MainViewModel : Observable
    {
        private bool _isBusy;
        private string _statusText;

        private ObservableCollection<Food> _foods;
        private Food _editableFood;
        private Food _selectedFood;
        private bool _isInternet;

        private RelayCommand _loadFoodsCommand;
        private RelayCommand _newFoodCommand;
        private RelayCommand _saveFoodCommand;
        private RelayCommand _deleteFoodCommand;

        private readonly FoodService _service;

        public MainViewModel()
        {
            _service = new FoodService();

            IsInternet = InternetConnection.Instance.IsInternetAvailable;

            InternetConnection.Instance.OnInternetAvailabilityChange += async (isInternet) =>
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => IsInternet = isInternet);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
                LoadFoodsCommand.OnCanExecuteChanged();
                NewFoodCommand.OnCanExecuteChanged();
                SaveFoodCommand.OnCanExecuteChanged();
                DeleteFoodCommand.OnCanExecuteChanged();
            }
        }

        public string StatusText
        {
            get => _statusText;
            set => Set(ref _statusText, value);
        }

        public ObservableCollection<Food> Foods
        {
            get => _foods;
            set => Set(ref _foods, value);
        }

        public Food EditableFood
        {
            get => _editableFood ?? (_editableFood = new Food());
            set
            {
                Set(ref _editableFood, value);
                DeleteFoodCommand.OnCanExecuteChanged();
                SaveFoodCommand.OnCanExecuteChanged();
            }
        }

        public Food SelectedFood
        {
            get => _selectedFood;
            set
            {
                Set(ref _selectedFood, value);
                EditableFood = value;
            }
        }

        public bool IsInternet
        {
            get => _isInternet;
            set
            {
                Set(ref _isInternet, value);
                OnPropertyChanged(nameof(NoInternet));
            }
        }

        public bool NoInternet => !IsInternet;

        public RelayCommand LoadFoodsCommand => _loadFoodsCommand ?? (_loadFoodsCommand = new RelayCommand(async () => await LoadFoodsAsync(), () => !IsBusy));

        public RelayCommand NewFoodCommand => _newFoodCommand ?? (_newFoodCommand = new RelayCommand(() => EditableFood = new Food(), () => !IsBusy));

        public RelayCommand SaveFoodCommand => _saveFoodCommand ?? (_saveFoodCommand = new RelayCommand(async () => await SaveFoodAsync(), () => !IsBusy));

        public RelayCommand DeleteFoodCommand => _deleteFoodCommand ?? (_deleteFoodCommand = new RelayCommand(async () => await DeleteFoodAsync(), () => !IsBusy && CanDeleteFood()));
                
        private async Task RefreshFoodsAsync(string selectedId = null)
        {
            StatusText = "Loading food...";

            var selectedItemId = selectedId ?? SelectedFood?.Id;

            var foods = await _service.ReadAsync();
            Foods = new ObservableCollection<Food>(foods);
            SelectedFood = Foods.FirstOrDefault(i => i.Id == selectedItemId);
        }

        private async Task LoadFoodsAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await RefreshFoodsAsync();
                IsBusy = false;

                StatusText = "Finished load foods!.";
            }
        }
                
        private async Task SaveFoodAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                StatusText = "Save food...";

                await _service.AddOrUpdateAsync(EditableFood);
                await RefreshFoodsAsync(EditableFood.Id);

                IsBusy = false;
                StatusText = "Food saved completed!.";
            }
        }

        private async Task DeleteFoodAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                StatusText = "Remove food...";

                await _service.DeleteAsync(EditableFood);
                await RefreshFoodsAsync();

                IsBusy = false;
                StatusText = "Remove food completed!.";
            }
        }

        private bool CanDeleteFood() => !string.IsNullOrEmpty(EditableFood?.Id);
    }
}
