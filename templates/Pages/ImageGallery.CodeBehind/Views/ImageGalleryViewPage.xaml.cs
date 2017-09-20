using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        public ImageGalleryViewPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<SampleImage> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGallerySampleData();
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            NavigationService.Navigate<ImageGalleryViewDetailPage>(e.ClickedItem);
        }
    }
}
