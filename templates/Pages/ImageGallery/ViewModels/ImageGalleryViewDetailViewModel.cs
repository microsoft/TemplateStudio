using System;
using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.ViewModels
{
    public class ImageGalleryViewDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private SampleImage _selectedImage;

        public SampleImage SelectedImage
        {
            get => _selectedImage;
            set => Set(ref _selectedImage, value);
        }

        public ImageGalleryViewDetailViewModel()
        {
        }
    }
}
