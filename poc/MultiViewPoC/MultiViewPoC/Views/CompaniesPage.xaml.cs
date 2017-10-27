using System;

using MultiViewPoC.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiViewPoC.Views
{
    public sealed partial class CompaniesPage : Page
    {
        public CompaniesViewModel ViewModel { get; } = new CompaniesViewModel();

        public CompaniesPage()
        {
            InitializeComponent();
            Loaded += CompaniesPage_Loaded;
        }

        private async void CompaniesPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
