using System;
using System.IO;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

namespace Param_ItemNamespace.Helpers
{
    public static class SettingsStorageExtensions
    {
        // Use this extension methods to store and retrieve in local and roaming app data
        // For more info regarding storing and retrieving app data,
        // Documentation: https://docs.microsoft.com/windows/uwp/app-settings/store-and-retrieve-app-data

        private const string fileExtension = ".json";

        public static bool IsRoamingStorageAvailable(this ApplicationData appData)
        {
            return (appData.RoamingStorageQuota == 0);
        }

        public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
        {
            var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);
            var fileContent = await Json.StringifyAsync(content);

            await FileIO.WriteTextAsync(file, fileContent);
        }

        public static async Task<T> ReadAsync<T>(this StorageFolder folder, string name)
        {
            if (!File.Exists(Path.Combine(folder.Path, GetFileName(name))))
            {
                return default(T);
            }

            var file = await folder.GetFileAsync($"{name}.json");
            var fileContent = await FileIO.ReadTextAsync(file);

            return await Json.ToObjectAsync<T>(fileContent);
        }

        public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value)
        {
            settings.Values[key] = await Json.StringifyAsync(value);
        }

        public static async Task<T> ReadAsync<T>(this ApplicationDataContainer settings, string key)
        {
            object obj = null;

            if (settings.Values.TryGetValue(key, out obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }

            return default(T);
        }

         public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if(content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name is null or empty. Specify a valid file name", "fileName");
            }

            var storageFile = await folder.CreateFileAsync(fileName, options);
            await FileIO.WriteBytesAsync(storageFile, content);
            return storageFile;
        }

        public static async Task<byte[]> ReadFileAsync(this StorageFolder folder, string fileName)
        {
            var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);

            if((item != null) && item.IsOfType(StorageItemTypes.File))
            {
                var storageFile = await folder.GetFileAsync(fileName);
                byte[] content = await storageFile.ReadBytesAsync();
                return content;
            }

            return null;
        }

        public static async Task<byte[]> ReadBytesAsync(this StorageFile file)
        {
            if (file != null)
            {

                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    using (var reader = new DataReader(stream.GetInputStreamAt(0)))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        var bytes = new byte[stream.Size];
                        reader.ReadBytes(bytes);
                        return bytes;
                    }
                }
            }

            return null;
        }

        private static string GetFileName(string name)
        {
            return string.Concat(name, fileExtension);
        }
    }
}
