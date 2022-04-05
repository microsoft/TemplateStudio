using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

using Param_RootNamespace.Models;

namespace Param_RootNamespace.Helpers
{
    // TODO: Start sharing data from your pages / views with these steps:
    // - Step 1. Setup a DataTransferManager object in your page / view and add a DataRequested event handler
    //   (i.e. OnDataRequested) to be called whenever the user invokes share.
    // - Step 2. Within the OnDataRequested event handler create a ShareSourceData instance and add the data you want to share.
    // - Step 3. Call the SetData extension method before leaving the event handler (i.e. args.Request.SetData(shareSourceData))
    // - Step 4. Call the DataTransferManager.ShowShareUI method from your command or handler to start the sharing action
    // Also consider registering for the DataPackage ShareComplete event handler (args.Request.Data.ShareCompleted) to run code when the sharing operation ends. Be sure to unregister the ShareComplete event handler when done.
    // More details on sharing data at https://docs.microsoft.com/windows/uwp/app-to-app/share-data
    public static class DataRequestExtensions
    {
        public static void SetData(this DataRequest dataRequest, ShareSourceFeatureData config)
        {
            var deferral = dataRequest.GetDeferral();
            try
            {
                var requestData = dataRequest.Data;
                requestData.Properties.Title = config.Title;
                if (!string.IsNullOrEmpty(config.Description))
                {
                    requestData.Properties.Description = config.Description;
                }

                var storageItems = new List<IStorageItem>();
                foreach (var dataItem in config.Items)
                {
                    switch (dataItem.DataType)
                    {
                        case ShareSourceFeatureItemType.Text:
                            requestData.SetText(dataItem.Text);
                            break;
                        case ShareSourceFeatureItemType.WebLink:
                            requestData.SetWebLink(dataItem.WebLink);
                            break;
                        case ShareSourceFeatureItemType.ApplicationLink:
                            requestData.SetApplicationLink(dataItem.ApplicationLink);
                            break;
                        case ShareSourceFeatureItemType.Html:
                            var htmlFormat = HtmlFormatHelper.CreateHtmlFormat(dataItem.Html);
                            requestData.SetHtmlFormat(htmlFormat);
                            break;
                        case ShareSourceFeatureItemType.Image:
                            requestData.FillImage(dataItem.Image, storageItems);
                            break;
                        case ShareSourceFeatureItemType.StorageItems:
                            requestData.FillStorageItems(dataItem.StorageItems, storageItems);
                            break;
                        case ShareSourceFeatureItemType.DeferredContent:
                            requestData.FillDeferredContent(dataItem.DeferredDataFormatId, dataItem.GetDeferredDataAsyncFunc);
                            break;
                    }
                }

                if (storageItems.Any())
                {
                    requestData.SetStorageItems(storageItems);
                }
            }
            finally
            {
                deferral.Complete();
            }
        }

        private static void FillImage(this DataPackage requestData, StorageFile image, List<IStorageItem> storageItems)
        {
            storageItems.Add(image);
            var streamReference = RandomAccessStreamReference.CreateFromFile(image);
            requestData.Properties.Thumbnail = streamReference;
            requestData.SetBitmap(streamReference);
        }

        private static void FillStorageItems(this DataPackage requestData, IEnumerable<IStorageItem> sourceItems, List<IStorageItem> storageItems)
        {
            foreach (var item in sourceItems)
            {
                storageItems.Add(item);
            }
        }

        private static void FillDeferredContent(this DataPackage requestData, string deferredDataFormatId, Func<Task<object>> getDeferredDataAsyncFunc)
        {
            requestData.SetDataProvider(deferredDataFormatId, async (providerRequest) =>
            {
                var deferral = providerRequest.GetDeferral();
                try
                {
                    var deferredData = await getDeferredDataAsyncFunc();
                    providerRequest.SetData(deferredData);
                }
                finally
                {
                    deferral.Complete();
                }
            });
        }
    }
}
