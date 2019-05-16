using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Windows.UI.Xaml.Controls;

using Prism.Commands;
using Prism.Windows.Navigation;

namespace Param_RootNamespace.ViewModels
{
    public class ImageGalleryViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;
        private readonly IConnectedAnimationService _connectedAnimationService;

        public const string ImageGalleryViewSelectedIdKey = "ImageGalleryViewSelectedIdKey";

        private ObservableCollection<SampleImage> _source;
        private ICommand _itemSelectedCommand;

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Param_Setter(ref _source, value);
        }

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new DelegateCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance, IConnectedAnimationService connectedAnimationService)
        {
            _navigationService = navigationServiceInstance;
            _sampleDataService = sampleDataServiceInstance;
            _connectedAnimationService = connectedAnimationService;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            // TODO WTS: Replace this with your actual data
            Source = await _sampleDataService.GetGallerySampleDataAsync();
        }

        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            ImagesNavigationHelper.AddImageId(ImageGalleryViewSelectedIdKey, selected.ID);
            _connectedAnimationService.SetListDataItemForNextConnectedAnimation(selected);
            _navigationService.Navigate(PageTokens.ImageGalleryViewDetailPage, selected.ID);
        }
    }
}
