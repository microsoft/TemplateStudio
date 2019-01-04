using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ContentGridViewDetailPage : Page
    {
        public ContentGridViewDetailPage()
        {
            InitializeComponent();
        }

        private ContentGridViewDetailViewModel ViewModel => DataContext as ContentGridViewDetailViewModel;
    }
}
