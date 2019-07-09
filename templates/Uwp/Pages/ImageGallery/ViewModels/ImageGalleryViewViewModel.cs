using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Toolkit.Uwp.UI.Animations;

using Param_RootNamespace.Helpers;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.ViewModels
{
    public class ImageGalleryViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public const string ImageGalleryViewSelectedIdKey = "ImageGalleryViewSelectedIdKey";

        private ICommand _itemSelectedCommand;

        public ObservableCollection<SampleImage> Source { get; } = new ObservableCollection<SampleImage>();

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await SampleDataService.GetImageGalleryDataAsync("ms-appx:///Assets");

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            ImagesNavigationHelper.AddImageId(ImageGalleryViewSelectedIdKey, selected.ID);
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(selected);
        }
    }
}