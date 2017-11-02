using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Param_ItemNamespace.Services
{
    public static class ShareSource
    {
        // See more documentation about how to share data from your app
        // https://docs.microsoft.com/windows/uwp/app-to-app/share-data
        private static Action<DataRequestedEventArgs> _fillShareContentAction;
        private static DataTransferManager dataTransferManager;

        public static void Initialize()
        {
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        public static void ShareText(string textToShare, string title, string description = null)
        {
            _fillShareContentAction = (args) =>
            {
                var requestData = args.Request.Data;

                SetCommonProperties(requestData, title, description);
                requestData.SetText(textToShare);
            };

            DataTransferManager.ShowShareUI();
        }

        public static void ShareWebLink(Uri uriToShare, string title, string description = null)
        {
            _fillShareContentAction = (args) =>
            {
                var requestData = args.Request.Data;

                SetCommonProperties(requestData, title, description);
                requestData.SetWebLink(uriToShare);
            };

            DataTransferManager.ShowShareUI();
        }

        public static void ShareApplicationLink(Uri applicationLinkToShare, string title, string description = null)
        {
            // TODO WTS: If you want to share a link to your application be sure of the following
            //
            // Protocol must be added in Package.appxmanifest in Declarations/protocol
            // i.e.
            // <uap:Protocol Name="my-app-name">
            //      <uap:Logo>Assets\smallTile-sdk.png</uap:Logo>
            //      <uap:DisplayName>MyApp</uap:DisplayName>
            // </uap:Protocol>
            //
            // applicationLinkToShare must belong to the registered protocol
            // new Uri("my-app-name:navigate?page=MainPage")
            _fillShareContentAction = (args) =>
            {
                var requestData = args.Request.Data;

                SetCommonProperties(requestData, title, description);
                requestData.SetApplicationLink(applicationLinkToShare);
            };

            DataTransferManager.ShowShareUI();
        }

        public static void ShareHtml(string htmlToShare, string title, string description = null)
        {
            _fillShareContentAction = (args) =>
            {
                var requestData = args.Request.Data;

                SetCommonProperties(requestData, title, description);
                var htmlFormat = HtmlFormatHelper.CreateHtmlFormat(htmlToShare);
                requestData.SetHtmlFormat(htmlFormat);
            };

            DataTransferManager.ShowShareUI();
        }

        public static void ShareImage(StorageFile imageToShare, string title, string description = null)
        {
            _fillShareContentAction = (args) =>
            {
                var requestData = args.Request.Data;

                SetCommonProperties(requestData, title, description);
                var imageItems = new List<IStorageItem>();
                imageItems.Add(imageToShare);
                requestData.SetStorageItems(imageItems);

                var imageStreamRef = RandomAccessStreamReference.CreateFromFile(imageToShare);
                requestData.Properties.Thumbnail = imageStreamRef;
                requestData.SetBitmap(imageStreamRef);
            };

            DataTransferManager.ShowShareUI();
        }

        public static void ShareDeferredContent(string formatId, Func<Task<object>> getDeferredDataAsync, string title, string description = null)
        {
            // Use this method to share content when you want not process the data until the target app actually requests it.
            // formatId must be a const value from StandardDataFormats class.
            // getDeferredDataAsync is the function that returns the object you want to share.
            if (getDeferredDataAsync == null)
            {
                throw new ArgumentNullException(nameof(getDeferredDataAsync));
            }

            _fillShareContentAction = (args) =>
            {
                var requestData = args.Request.Data;

                SetCommonProperties(requestData, title, description);
                requestData.SetDataProvider(formatId, async (providerRequest) =>
                {
                    var deferral = providerRequest.GetDeferral();
                    try
                    {
                        var data = await getDeferredDataAsync();
                        providerRequest.SetData(data);
                    }
                    finally
                    {
                        deferral.Complete();
                    }
                });
            };

            DataTransferManager.ShowShareUI();
        }

        private static void SetCommonProperties(DataPackage requestData, string title, string description = null)
        {
            requestData.Properties.Title = title;
            requestData.Properties.Description = description;
        }

        private static void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // This event will be fired when share operation starts.
            // We need to add data on DataRequestedEventArgs throw the _fillShareContentAction
            args.Request.Data.ShareCompleted += OnShareCompleted;
            var deferral = args.Request.GetDeferral();
            try
            {
                _fillShareContentAction?.Invoke(args);
            }
            finally
            {
                deferral.Complete();
            }
        }

        private static void OnShareCompleted(DataPackage sender, ShareCompletedEventArgs args)
        {
            // This event will be fired when Share Operation will finish
            // TODO WTS: If you need to handle any action when de data is shared implement on this method
        }
    }
}