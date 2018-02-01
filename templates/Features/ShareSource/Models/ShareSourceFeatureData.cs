using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Param_ItemNamespace.Helpers;
using Windows.Storage;

namespace Param_ItemNamespace.Models
{
    public class ShareSourceFeatureData
    {
        public string Title { get; set; }

        public string Description { get; set; }

        internal List<ShareSourceFeatureItem> Items { get; }

        public ShareSourceFeatureData(string title, string desciption = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("ExceptionShareSourceFeatureDataTitleIsNullOrEmpty".GetLocalized(), nameof(title));
            }

            Items = new List<ShareSourceFeatureItem>();
            Title = title;
            Description = desciption;
        }

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("ExceptionShareSourceFeatureDataTitleIsNullOrEmpty".GetLocalized(), nameof(text));
            }

            Items.Add(ShareSourceFeatureItem.FromText(text));
        }

        public void SetWebLink(Uri webLink)
        {
            if (webLink == null)
            {
                throw new ArgumentNullException(nameof(webLink));
            }

            Items.Add(ShareSourceFeatureItem.FromWebLink(webLink));
        }

        // TODO WTS: To share a link to your application be sure you have configured activation by URI
        //
        // 1. Register the protocol in Package.appxmanifest Declarations/protocol
        //      i.e.
        //      <uap:Protocol Name="my-app-name">
        //          <uap:Logo>Assets\smallTile-sdk.png</uap:Logo>
        //          <uap:DisplayName>MyApp</uap:DisplayName>
        //      </uap:Protocol>
        //
        // 2. The applicationLink parameter must refer to the registered protocol (i.e. new Uri("my-app-name:navigate?page=MainPage"))
        public void SetApplicationLink(Uri applicationLink)
        {
            if (applicationLink == null)
            {
                throw new ArgumentNullException(nameof(applicationLink));
            }

            Items.Add(ShareSourceFeatureItem.FromApplicationLink(applicationLink));
        }

        public void SetHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentException("ExceptionShareSourceFeatureDataHtmlIsNullOrEmpty".GetLocalized(), nameof(html));
            }

            Items.Add(ShareSourceFeatureItem.FromHtml(html));
        }

        public void SetImage(StorageFile image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            Items.Add(ShareSourceFeatureItem.FromImage(image));
        }

        public void SetStorageItems(IEnumerable<IStorageItem> storageItems)
        {
            if (storageItems == null || !storageItems.Any())
            {
                throw new ArgumentException("ExceptionShareSourceFeatureDataStorageItemsIsNullOrEmpty".GetLocalized(), nameof(storageItems));
            }

            Items.Add(ShareSourceFeatureItem.FromStorageItems(storageItems));
        }

        // Use this method to add content to share when you do not want to process the data until the target app actually requests it.
        // The defferedDataFormatId parameter must be a const value from StandardDataFormats class.
        // The getDeferredDataAsyncFunc parameter is the function that returns the object you want to share.
        public void SetDeferredContent(string deferredDataFormatId, Func<Task<object>> getDeferredDataAsyncFunc)
        {
            if (string.IsNullOrEmpty(deferredDataFormatId))
            {
                throw new ArgumentException("ExceptionShareSourceFeatureDataDeferredDataFormatIdIsNullOrEmpty".GetLocalized(), nameof(deferredDataFormatId));
            }

            Items.Add(ShareSourceFeatureItem.FromDeferredContent(deferredDataFormatId, getDeferredDataAsyncFunc));
        }
    }
}
