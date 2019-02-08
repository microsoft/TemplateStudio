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
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Models;

namespace Param_RootNamespace.Views
{
    // TODO WTS: Remove this example page when/if it's not needed.
    // This page is an example of how to handle data that is shared with your app.
    // You can either change this page to meet your needs, or use another and delete this page.
    public sealed partial class ShareTargetFeaturePage : Page, INotifyPropertyChanged
    {
        private ShareOperation _shareOperation;

        private SharedDataModelBase _sharedData;

        public SharedDataModelBase SharedData
        {
            get => _sharedData;
            set => Set(ref _sharedData, value);
        }

        public ShareTargetFeaturePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO WTS: Configure the Share Target Declaration for the formats you require.
            // Share Target declarations are defined in Package.appxmanifest.
            // Current declarations allow to share WebLink and image files with the app.
            // ShareTarget can be tested sharing the WebLink from Microsoft Edge or sharing images from Windows Photos.

            // ShareOperation contains all the information required to handle the action.
            base.OnNavigatedTo(e);

            // TODO WTS: Customize SharedDataModelBase or derived classes adding properties for data that you need to extract from _shareOperation
            _shareOperation = e.Parameter as ShareOperation;

            if (_shareOperation.Data.Contains(StandardDataFormats.WebLink))
            {
                var newSharedData = new SharedDataWebLinkModel()
                {
                    Title = _shareOperation.Data.Properties.Title,
                    PageTitle = "ShareTargetFeature_WebLinkTitle".GetLocalized(),
                    DataFormat = StandardDataFormats.WebLink
                };
                newSharedData.Uri = await _shareOperation.GetWebLinkAsync();
                SharedData = newSharedData;
            }

            if (_shareOperation.Data.Contains(StandardDataFormats.StorageItems))
            {
                var newSharedData = new SharedDataStorageItemsModel()
                {
                    Title = _shareOperation.Data.Properties.Title,
                    PageTitle = "ShareTargetFeature_ImagesTitle".GetLocalized(),
                    DataFormat = StandardDataFormats.StorageItems
                };
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

                SharedData = newSharedData;
            }
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
            // TODO WTS: Implement any other logic or add a QuickLink before completing the share operation.
            // More details at https://docs.microsoft.com/en-us/windows/uwp/app-to-app/receive-data
            _shareOperation.ReportCompleted();
        }
    }
}
