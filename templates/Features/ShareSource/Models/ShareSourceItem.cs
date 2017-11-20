using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Storage;

namespace Param_ItemNamespace.Models
{
    internal enum ShareSourceItemType
    {
        Text = 0,
        WebLink = 1,
        ApplicationLink = 2,
        Html = 3,
        Image = 4,
        StorageItems = 5,
        DeferredContent = 6
    }

    internal class ShareSourceItem
    {
        public ShareSourceItemType DataType { get; }

        public string Text { get; private set; }

        public Uri WebLink { get; private set; }

        public Uri ApplicationLink { get; private set; }

        public string Html { get; private set; }

        public StorageFile Image { get; private set; }

        public IEnumerable<IStorageItem> StorageItems { get; private set; }

        public string DeferredDataFormatId { get; private set; }

        public Func<Task<object>> GetDeferredDataAsyncFunc { get; private set; }

        private ShareSourceItem(ShareSourceItemType dataType)
        {
            DataType = dataType;
        }

        internal static ShareSourceItem FromText(string text)
        {
            return new ShareSourceItem(ShareSourceItemType.Text)
            {
                Text = text
            };
        }

        internal static ShareSourceItem FromWebLink(Uri webLink)
        {
            return new ShareSourceItem(ShareSourceItemType.WebLink)
            {
                WebLink = webLink
            };
        }

        internal static ShareSourceItem FromApplicationLink(Uri applicationLink)
        {
            return new ShareSourceItem(ShareSourceItemType.ApplicationLink)
            {
                ApplicationLink = applicationLink
            };
        }

        internal static ShareSourceItem FromHtml(string html)
        {
            return new ShareSourceItem(ShareSourceItemType.Html)
            {
                Html = html
            };
        }

        internal static ShareSourceItem FromImage(StorageFile image)
        {
            return new ShareSourceItem(ShareSourceItemType.Image)
            {
                Image = image
            };
        }

        internal static ShareSourceItem FromStorageItems(IEnumerable<IStorageItem> storageItems)
        {
            return new ShareSourceItem(ShareSourceItemType.StorageItems)
            {
                StorageItems = storageItems
            };
        }

        internal static ShareSourceItem FromDeferredContent(string deferredDataFormatId, Func<Task<object>> getDeferredDataAsyncFunc)
        {
            return new ShareSourceItem(ShareSourceItemType.DeferredContent)
            {
                DeferredDataFormatId = deferredDataFormatId,
                GetDeferredDataAsyncFunc = getDeferredDataAsyncFunc
            };
        }
    }
}
