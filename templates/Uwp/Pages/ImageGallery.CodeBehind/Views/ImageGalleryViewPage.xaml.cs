using System;
using System.Collections.ObjectModel;

using Microsoft.Toolkit.Uwp.UI.Animations;

using Windows.UI.Xaml.Controls;

using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.Core.Services;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        public const string ImageGalleryViewSelectedIdKey = "ImageGalleryViewSelectedIdKey";

        private ObservableCollection<SampleImage> _source;

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Param_Setter(ref _source, value);
        }

        public ImageGalleryViewPage()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
            InitializeComponent();
        }

        private void ImagesGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selected = e.ClickedItem as SampleImage;
            ImagesNavigationHelper.AddImageId(ImageGalleryViewSelectedIdKey, selected.ID);
            NavigationService.Frame.SetListDataItemForNextConnectedAnnimation(selected);
            NavigationService.Navigate<ImageGalleryViewDetailPage>(selected.ID);
        }
    }
}
