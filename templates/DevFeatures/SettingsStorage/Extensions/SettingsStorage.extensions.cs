using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace ItemNamespace.Extensions
{
    public static class SettingsStorageExtensions
    {
        //For mor info regarding storing and retrieving app data, 
        //please see: https://docs.microsoft.com/windows/uwp/app-settings/store-and-retrieve-app-data
        
        private const string fileExtension = ".json";

        public static bool IsRoamingStorageAvailable(this ApplicationData appData)
        {
            return (appData.RoamingStorageQuota == 0);
        }

        public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
        {
            var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);

            var fileContent = JsonConvert.SerializeObject(content);

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

            return JsonConvert.DeserializeObject<T>(fileContent);
        }

        public static void Save<T>(this ApplicationDataContainer settings, string key, T value)
        {
            if (settings.Values.ContainsKey(key))
            {
                settings.Values[key] = JsonConvert.SerializeObject(value, Formatting.None);
            }
            else
            {
                settings.Values.Add(key, JsonConvert.SerializeObject(value));
            }
        }

        public static T Read<T>(this ApplicationDataContainer settings, string key)
        {
            if (settings.Values.ContainsKey(key))
            {
                var value = (string)settings.Values[key];
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<T>(value);
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
