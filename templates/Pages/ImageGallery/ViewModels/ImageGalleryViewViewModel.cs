using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class ImageGalleryViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private ICommand _itemSelectedCommand;

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewViewModel()
        {
        }

        public ObservableCollection<SampleImage> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGallerySampleData();
            }
        }
    }
}
