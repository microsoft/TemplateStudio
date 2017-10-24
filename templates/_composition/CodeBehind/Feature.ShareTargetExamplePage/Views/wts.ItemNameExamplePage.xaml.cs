using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.Views
{
    // TODO WTS: This page exists purely as an example of how to launch a specific page in response to a protocol launch and pass it a value. It is expected that you will delete this page once you have changed the handling of a protocol launch to meet your needs and redirected to another of your pages.
    public sealed partial class wts.ItemNameExamplePage : Page, INotifyPropertyChanged
    {
        private ShareOperation _shareOperation;

        private string _pageTitle;

        public string PageTitle
        {
            get => _pageTitle;
            set => Set(ref _pageTitle, value);
        }

        private SharedContentModel _sharedData;

        public SharedContentModel SharedData
        {
            get => _sharedData;
            set => Set(ref _sharedData, value);
        }

        public wts.ItemNameExamplePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // ShareTarget declarations are defined in Package.appxmanifest.
            // Current declarations allow tho share WebLink and image files with the app.
            // ShareTarget can be tested sharing the WebLink from Microsoft Edge or sharing images from Windows Photos.
            base.OnNavigatedTo(e);
            _shareOperation = e.Parameter as ShareOperation;
            var newSharedData = new SharedContentModel
            {
                Title = _shareOperation.Data.Properties.Title
            };

            if (_shareOperation.Data.Contains(StandardDataFormats.WebLink))
            {
                PageTitle = "ShareTargetExample_WebLinkTitle".GetLocalized();
                newSharedData.DataFormat = StandardDataFormats.WebLink;
                newSharedData.Uri = await _shareOperation.GetWebLinkAsync();
            }

            if (_shareOperation.Data.Contains(StandardDataFormats.StorageItems))
            {
                PageTitle = "ShareTargetExample_ImagesTitle".GetLocalized();
                newSharedData.DataFormat = StandardDataFormats.StorageItems;
                var files = await _shareOperation.GetStorageItemsAsync();
                foreach (var file in files)
                {
                    var storageFile = file as StorageFile;
                    if (storageFile != null)
                    {
                        using (var inputStream = await storageFile.OpenReadAsync())
                        {
                            var img = new BitmapImage();
                            img.SetSource(inputStream);
                            newSharedData.Images.Add(img);
                        }
                    }
                }
            }

            SharedData = newSharedData;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            _shareOperation.ReportCompleted();
        }
    }
}
