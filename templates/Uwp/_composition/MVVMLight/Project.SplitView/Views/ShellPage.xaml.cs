using System;
using wts.ItemName.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using wts.ItemName.ViewModels;

namespace wts.ItemName.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel
        {
            get { return DataContext as ShellViewModel; }
        }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);
        }
    }
}
