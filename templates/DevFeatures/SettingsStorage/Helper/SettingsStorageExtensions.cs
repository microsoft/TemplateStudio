using System;
using System.IO;
using System.Threading.Tasks;

using Windows.Storage;

namespace ItemNamespace.Helper
{
    public static class SettingsStorageExtensions
    {
        // TODO UWPTemplates: Use this extension methods to store and retrieve in local and roaming app data 
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
            if (settings.Values.ContainsKey(key))
            {
                settings.Values[key] = await Json.StringifyAsync(value);
            }
            else
            {
                settings.Values.Add(key, await Json.StringifyAsync(value));
            }
        }

        public static async Task<T> ReadAsync<T>(this ApplicationDataContainer settings, string key)
        {
            if (settings.Values.ContainsKey(key))
            {
                var value = (string)settings.Values[key];

                if (value != null)
                {
                    return await Json.ToObjectAsync<T>(value);
                }
            }

            return default(T);
        }

        private static string GetFileName(string name)
        {
            return string.Concat(name, fileExtension);
        }
    }
}
