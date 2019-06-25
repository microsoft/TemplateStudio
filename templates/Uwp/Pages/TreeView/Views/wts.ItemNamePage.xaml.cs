using System;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Param_RootNamespace.Views
{
    // For more info about the TreeView Control see
    // https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/tree-view
    // For other samples, get the XAML Controls Gallery app http://aka.ms/XamlControlsGallery
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
