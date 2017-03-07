using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace RootNamespace.Services
{
    /// <summary>
    /// This service allows to save/retrieve settings and files to/from local and remote storage,
    /// Documentation: https://docs.microsoft.com/en-us/windows/uwp/app-settings/store-and-retrieve-app-data
    /// </summary>
    public static class SettingsStorageService
    {
        public static bool IsRoamingStorageAvailable
        {
            get
            {
                return (RoamingQuota == 0);
            }
        }
        public static ulong RoamingQuota
        {
            get
            {
                return ApplicationData.Current.RoamingStorageQuota;
            }
        }

        public static async Task SaveAsync<T>(string name, T content, StorageFolder folder)
        {
            var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);

            var fileContent = JsonConvert.SerializeObject(content);

            await FileIO.WriteTextAsync(file, fileContent);
        }

        public static async Task<T> ReadAsync<T>(string name, StorageFolder folder)
        {
            if (!File.Exists(Path.Combine(folder.Path, GetFileName(name))))
            {
                return default(T);
            }
            var file = await folder.GetFileAsync($"{name}.json");
            var fileContent = await FileIO.ReadTextAsync(file);

            return JsonConvert.DeserializeObject<T>(fileContent);
        }

        private static string GetFileName(string name)
        {
            return $"{name}.json";
        }

        public static void Save<T>(string key, T value, ApplicationDataContainer settings)
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

        public static T Read<T>(string key, ApplicationDataContainer settings)
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
    }
}
