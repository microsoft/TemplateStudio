using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
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

        private ContentGridViewDetailViewModel ViewModel => DataContext as ContentGridViewDetailViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.RegisterElementForConnectedAnimation("animationKeyContentGridView", itemHero);
        }
    }
}
