using System;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNameViewModel ViewModel { get; } = new wts.ItemNameViewModel();

        public wts.ItemNamePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadDataAsync();
        }
    }
}
