using System;
using System.Collections.ObjectModel;

using Microsoft.Toolkit.Uwp.UI.Animations;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Param_RootNamespace.Helpers;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.Views
{
    public sealed partial class ImageGalleryViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        public const string ImageGalleryViewSelectedIdKey = "ImageGalleryViewSelectedIdKey";

        public ObservableCollection<SampleImage> Source { get; } = new ObservableCollection<SampleImage>();

        public ImageGalleryViewPage()
        {
            InitializeComponent();
            Loaded += ImageGalleryViewPage_OnLoaded;
        }

        private async void ImageGalleryViewPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await SampleDataService.GetImageGalleryDataAsync("ms-appx:///Assets");

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        private void ImagesGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selected = e.ClickedItem as SampleImage;
            ImagesNavigationHelper.AddImageId(ImageGalleryViewSelectedIdKey, selected.ID);
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(selected);
            NavigationService.Navigate<ImageGalleryViewDetailPage>(selected.ID);
        }
    }
}
