using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.Views
{
    public sealed partial class ContentGridViewDetailPage : Page
    {
        public ContentGridViewDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.RegisterElementForConnectedAnimation("animationKeyContentGridView", itemHero);
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
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
