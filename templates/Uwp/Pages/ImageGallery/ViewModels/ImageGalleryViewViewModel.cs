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

        private ObservableCollection<SampleImage> _source;
        private ICommand _itemSelectedCommand;

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Param_Setter(ref _source, value);
        }

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            // TODO WTS: Replace this with your actual data
            Source = await SampleDataService.GetGallerySampleDataAsync();
        }

        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            ImagesNavigationHelper.AddImageId(ImageGalleryViewSelectedIdKey, selected.ID);
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(selected);
        }
    }
}