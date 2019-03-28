using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage.Streams;
using Windows.Storage;

namespace Param_RootNamespace.Helpers
{
    public static class ShareOperationExtensions
    {
        public static async Task<Uri> GetApplicationLinkAsync(this ShareOperation shareOperation)
        {
            return await GetOperationDataAsync<Uri>(shareOperation, StandardDataFormats.ApplicationLink);
        }

        public static async Task<RandomAccessStreamReference> GetBitmapAsync(this ShareOperation shareOperation)
        {
            return await GetOperationDataAsync<RandomAccessStreamReference>(shareOperation, StandardDataFormats.Bitmap);
        }

        public static async Task<string> GetHtmlFormatAsync(this ShareOperation shareOperation)
        {
            return await GetOperationDataAsync<string>(shareOperation, StandardDataFormats.Html);
        }

        public static async Task<string> GetRtfAsync(this ShareOperation shareOperation)
        {
            return await GetOperationDataAsync<string>(shareOperation, StandardDataFormats.Rtf);
        }

        public static async Task<IReadOnlyList<IStorageItem>> GetStorageItemsAsync(this ShareOperation shareOperation)
        {
            return await GetOperationDataAsync<IReadOnlyList<IStorageItem>>(shareOperation, StandardDataFormats.StorageItems);
        }

        public static async Task<string> GetTextAsync(this ShareOperation shareOperation)
        {
            return await GetOperationDataAsync<string>(shareOperation, StandardDataFormats.Text);
        }

        public static async Task<Uri> GetWebLinkAsync(this ShareOperation shareOperation)
        {
            return await GetOperationDataAsync<Uri>(shareOperation, StandardDataFormats.WebLink) as Uri;
        }

        private static async Task<T> GetOperationDataAsync<T>(this ShareOperation shareOperation, string dataFormat)
            where T : class
        {
            try
            {
                if (dataFormat == StandardDataFormats.ApplicationLink)
                {
                    return await shareOperation.Data.GetApplicationLinkAsync() as T;
                }

                if (dataFormat == StandardDataFormats.Bitmap)
                {
                    return await shareOperation.Data.GetBitmapAsync() as T;
                }

                if (dataFormat == StandardDataFormats.Html)
                {
                    return await shareOperation.Data.GetHtmlFormatAsync() as T;
                }

                if (dataFormat == StandardDataFormats.Rtf)
                {
                    return await shareOperation.Data.GetRtfAsync() as T;
                }

                if (dataFormat == StandardDataFormats.StorageItems)
                {
                    return await shareOperation.Data.GetStorageItemsAsync() as T;
                }

                if (dataFormat == StandardDataFormats.Text)
                {
                    return await shareOperation.Data.GetTextAsync() as T;
                }

                if (dataFormat == StandardDataFormats.WebLink)
                {
                    return await shareOperation.Data.GetWebLinkAsync() as T;
                }
            }
            catch (Exception)
            {
                return default(T);
            }

            return default(T);
        }
    }
}
