using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.Extensions
{
    public static class DataRequestExtensions
    {
        // TODO WTS: This Extension method helps to fill de DataRequest in DataTransferManager share operation.
        // See more details about share contract in UWP
        // https://docs.microsoft.com/windows/uwp/app-to-app/share-data
        public static void SetData(this DataRequest dataRequest, ShareSourceData config)
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
                        case ShareSourceItemType.Text:
                            requestData.SetText(dataItem.Text);
                            break;
                        case ShareSourceItemType.WebLink:
                            requestData.SetWebLink(dataItem.WebLink);
                            break;
                        case ShareSourceItemType.ApplicationLink:
                            requestData.SetApplicationLink(dataItem.ApplicationLink);
                            break;
                        case ShareSourceItemType.Html:
                            var htmlFormat = HtmlFormatHelper.CreateHtmlFormat(dataItem.Html);
                            requestData.SetHtmlFormat(htmlFormat);
                            break;
                        case ShareSourceItemType.Image:
                            requestData.FillImage(dataItem.Image, storageItems);
                            break;
                        case ShareSourceItemType.StorageItems:
                            requestData.FillStorageItems(dataItem.StorageItems, storageItems);
                            break;
                        case ShareSourceItemType.DeferredContent:
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
