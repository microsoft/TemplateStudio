using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using MultiViewPoC.Helpers;
using MultiViewPoC.Models;
using MultiViewPoC.Services;
using MultiViewPoC.Views;

using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MultiViewPoC.ViewModels
{
    public class ImagesViewModel : Observable
    {
        public const string ImagesSelectedImageId = "ImagesSelectedImageId";
        public const string ImagesAnimationOpen = "Images_AnimationOpen";
        public const string ImagesAnimationClose = "Images_AnimationClose";

        private ObservableCollection<SampleImage> _source;
        private ICommand _itemSelectedCommand;
        private GridView _imagesGridView;

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImagesViewModel()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
        }

        public async Task LoadAnimationAsync(GridView imagesGridView)
        {
            _imagesGridView = imagesGridView;
            var selectedImageId = await ApplicationData.Current.LocalSettings.ReadAsync<string>(ImagesSelectedImageId);
            if (!string.IsNullOrEmpty(selectedImageId))
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImagesAnimationClose);
                if (animation != null)
                {
                    var item = _imagesGridView.Items.FirstOrDefault(i => ((SampleImage)i).ID == selectedImageId);
                    _imagesGridView.ScrollIntoView(item);
                    await _imagesGridView.TryStartConnectedAnimationAsync(animation, item, "galleryImage");
                }

                ApplicationData.Current.LocalSettings.SaveString(ImagesSelectedImageId, string.Empty);
            }
        }

        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            _imagesGridView.PrepareConnectedAnimation(ImagesAnimationOpen, selected, "galleryImage");
            NavigationService.Navigate<ImagesDetailPage>(args.ClickedItem);
        }
    }
}
