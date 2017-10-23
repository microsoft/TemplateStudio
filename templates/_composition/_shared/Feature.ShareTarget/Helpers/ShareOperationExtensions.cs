using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;

namespace Param_ItemNamespace.Helpers
{
    public static class ShareOperationExtensions
    {
        public static async Task<IReadOnlyList<IStorageItem>> GetStorageItemsAsync(this ShareOperation shareOperation)
        {
            return await GetOperationDataAsync<IReadOnlyList<IStorageItem>>(shareOperation, StandardDataFormats.StorageItems);
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
                if (dataFormat == StandardDataFormats.WebLink)
                {
                    return await shareOperation.Data.GetWebLinkAsync() as T;
                }

                if (dataFormat == StandardDataFormats.StorageItems)
                {
                    return await shareOperation.Data.GetStorageItemsAsync() as T;
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
