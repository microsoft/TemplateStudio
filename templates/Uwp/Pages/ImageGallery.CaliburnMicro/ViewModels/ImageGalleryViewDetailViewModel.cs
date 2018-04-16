using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class ImageGalleryViewDetailViewModel : Screen
    {
        private static UIElement _image;
        private SampleImage _selectedImage;

        public SampleImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                Param_Setter(ref _selectedImage, value);
                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey, SelectedImage.ID);
            }
        }

        public string ID { get; set; }

        public BindableCollection<SampleImage> Source { get; } = new BindableCollection<SampleImage>();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // TODO WTS: Replace this with your actual data
            Source.AddRange(SampleDataService.GetGallerySampleData());
        }

        public async Task InitializeAsync(UIElement image, NavigationMode navigationMode)
        {
            _image = image;
            if (navigationMode == NavigationMode.New)
            {
                SelectedImage = Source.FirstOrDefault(i => i.ID == ID);
            }
            else
            {
                var selectedImageId = await ApplicationData.Current.LocalSettings.ReadAsync<string>(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey);
                if (!string.IsNullOrEmpty(selectedImageId))
                {
                    SelectedImage = Source.FirstOrDefault(i => i.ID == selectedImageId);
                }
            }

            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewViewModel.ImageGalleryViewAnimationOpen);
            animation?.TryStart(_image);
        }

        public void SetAnimation()
        {
            ConnectedAnimationService.GetForCurrentView()?.PrepareToAnimate(ImageGalleryViewViewModel.ImageGalleryViewAnimationClose, _image);
        }
    }
}
