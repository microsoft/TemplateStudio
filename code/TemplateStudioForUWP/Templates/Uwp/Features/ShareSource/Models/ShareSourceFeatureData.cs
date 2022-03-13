using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Param_RootNamespace.Helpers;
using Windows.Storage;

namespace Param_RootNamespace.Models
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
                throw new ArgumentException("The parameter title can not be null or empty.", nameof(title));
            }

            Items = new List<ShareSourceFeatureItem>();
            Title = title;
            Description = desciption;
        }

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The parameter title can not be null or empty.", nameof(text));
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

        // To share a link to your app you must first register it to handle URI activation
        // More details at https://docs.microsoft.com/windows/uwp/launch-resume/handle-uri-activation
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
                throw new ArgumentException("The Parameter html is null or empty.", nameof(html));
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
                throw new ArgumentException("The Parameter StorageItems is null or does not contains any element.", nameof(storageItems));
            }

            Items.Add(ShareSourceFeatureItem.FromStorageItems(storageItems));
        }

        // Use this method to add content to share when you do not want to process the data until the target app actually requests it.
        // The deferredDataFormatId parameter must be a const value from StandardDataFormats class.
        // The getDeferredDataAsyncFunc parameter is the function that returns the object you want to share.
        public void SetDeferredContent(string deferredDataFormatId, Func<Task<object>> getDeferredDataAsyncFunc)
        {
            if (string.IsNullOrEmpty(deferredDataFormatId))
            {
                throw new ArgumentException("The Parameter DeferredDataFormatId is null or does not contains any element.", nameof(deferredDataFormatId));
            }

            Items.Add(ShareSourceFeatureItem.FromDeferredContent(deferredDataFormatId, getDeferredDataAsyncFunc));
        }
    }
}
