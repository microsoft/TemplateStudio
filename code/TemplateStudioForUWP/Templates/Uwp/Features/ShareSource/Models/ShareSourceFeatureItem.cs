using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Storage;

namespace Param_RootNamespace.Models
{
    internal enum ShareSourceFeatureItemType
    {
        Text = 0,
        WebLink = 1,
        ApplicationLink = 2,
        Html = 3,
        Image = 4,
        StorageItems = 5,
        DeferredContent = 6
    }

    internal class ShareSourceFeatureItem
    {
        public ShareSourceFeatureItemType DataType { get; }

        public string Text { get; private set; }

        public Uri WebLink { get; private set; }

        public Uri ApplicationLink { get; private set; }

        public string Html { get; private set; }

        public StorageFile Image { get; private set; }

        public IEnumerable<IStorageItem> StorageItems { get; private set; }

        public string DeferredDataFormatId { get; private set; }

        public Func<Task<object>> GetDeferredDataAsyncFunc { get; private set; }

        private ShareSourceFeatureItem(ShareSourceFeatureItemType dataType)
        {
            DataType = dataType;
        }

        internal static ShareSourceFeatureItem FromText(string text)
        {
            return new ShareSourceFeatureItem(ShareSourceFeatureItemType.Text)
            {
                Text = text
            };
        }

        internal static ShareSourceFeatureItem FromWebLink(Uri webLink)
        {
            return new ShareSourceFeatureItem(ShareSourceFeatureItemType.WebLink)
            {
                WebLink = webLink
            };
        }

        internal static ShareSourceFeatureItem FromApplicationLink(Uri applicationLink)
        {
            return new ShareSourceFeatureItem(ShareSourceFeatureItemType.ApplicationLink)
            {
                ApplicationLink = applicationLink
            };
        }

        internal static ShareSourceFeatureItem FromHtml(string html)
        {
            return new ShareSourceFeatureItem(ShareSourceFeatureItemType.Html)
            {
                Html = html
            };
        }

        internal static ShareSourceFeatureItem FromImage(StorageFile image)
        {
            return new ShareSourceFeatureItem(ShareSourceFeatureItemType.Image)
            {
                Image = image
            };
        }

        internal static ShareSourceFeatureItem FromStorageItems(IEnumerable<IStorageItem> storageItems)
        {
            return new ShareSourceFeatureItem(ShareSourceFeatureItemType.StorageItems)
            {
                StorageItems = storageItems
            };
        }

        internal static ShareSourceFeatureItem FromDeferredContent(string deferredDataFormatId, Func<Task<object>> getDeferredDataAsyncFunc)
        {
            return new ShareSourceFeatureItem(ShareSourceFeatureItemType.DeferredContent)
            {
                DeferredDataFormatId = deferredDataFormatId,
                GetDeferredDataAsyncFunc = getDeferredDataAsyncFunc
            };
        }
    }
}
