using System;

using EasyTablesPoc.ViewModels;

using Windows.UI.Xaml.Controls;

namespace EasyTablesPoc.Views
{
    public sealed partial class TodoListPage : Page
    {
        public TodoListViewModel ViewModel { get; } = new TodoListViewModel();

        public TodoListPage()
        {
            InitializeComponent();
        }
    }
}
