using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNameExamplePage : Page
    {
        public wts.ItemNameExampleViewModel ViewModel { get; } = new wts.ItemNameExampleViewModel();

        public wts.ItemNameExamplePage()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadAsync(e.Parameter as ShareOperation);
        }
    }
}
