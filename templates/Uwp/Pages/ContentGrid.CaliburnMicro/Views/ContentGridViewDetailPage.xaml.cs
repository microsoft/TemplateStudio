using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    public sealed partial class ContentGridViewDetailPage : Page
    {
        public ContentGridViewDetailPage()
        {
            InitializeComponent();
        }

        private ContentGridViewDetailViewModel ViewModel
        {
            get { return DataContext as ContentGridViewDetailViewModel; }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is long orderID)
            {
                await ViewModel.InitializeAsync(orderID);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.SetListDataItemForNextConnectedAnimation();
            }
        }
    }
}
