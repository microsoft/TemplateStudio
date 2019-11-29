# Setting Storage Extensions

Settings Storage provides extension methods that make working with application data simpler. It handles loading, saving, and serializing both files and individual pieces of data in any of the application folders.

[Find out more about storage and the ApplicationData class.](https://docs.microsoft.com/uwp/api/windows.storage.applicationdata)

---

:heavy_exclamation_mark: There is also a version of [this document with code samples in VB.Net](./setting-storage.vb.md) :heavy_exclamation_mark: |
----------------------------------------------------------------------------------------------------------------------------------------------- |

SettingsStorageExtensions is a helper class that groups extension methods to facilitate the use of serialization, handling and storing of any content in the same way as simple settings for a consistent interface (for example: custom content, key-values, files, etc.).

## Advantages

- All methods of handling and storing content can be found in the same class and using a similar nomenclature.
- Facilitate the serialization and deserialization of data content and make it transparent for the developers.
- Using async methods that avoid problems in the serialization process (for example: big files that can take a long time to process).
- All methods automatically give default values back if the key you're trying to read doesn't exist.
- It makes working with byte arrays as easy as working with non-binary data (which can sometimes be complicated.)

This helper contains extension methods for handling and storing content in:

- StorageFolder: Manages folders and their contents and provides information about them.

ApplicationData contains LocalFolder, RoamingFolder, TemporaryFolder, LocalCacheFolder and SharedLocalFolder. [More info](https://docs.microsoft.com/uwp/api/Windows.Storage.StorageFolder).

- ApplicationDataContainer: It´s a container for app settings.

ApplicationData contains LocalSettings and RoamingSettings data containers.
We can also create our settings containers calling to  ApplicationDataContainer.CreateContainer. [More info](https://docs.microsoft.com/uwp/api/windows.storage.applicationdatacontainer).

More info: [https://docs.microsoft.com/windows/uwp/design/app-settings/store-and-retrieve-app-data](https://docs.microsoft.com/windows/uwp/design/app-settings/store-and-retrieve-app-data).

## Examples of use

To use these extension methods you have to add a reference to:

```csharp
using {YourAppName}.Helpers;
```

We will also use a example class as a content model:

```csharp
public class MyData
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Application Data Container

- Save to ApplicationDataContainer: saves MyData object as a key-value pair where the value is serialized in json.

```csharp
private async Task SaveMyDataAsync(string key, MyData data)
{
    //save in local settings
    await ApplicationData.Current.LocalSettings.SaveAsync(key, data);

    //save in roaming settings
    if (ApplicationData.Current.IsRoamingStorageAvailable())
    {
        await ApplicationData.Current.RoamingSettings.SaveAsync(key, data);
    }
}
```

- Get content from ApplicationData:

```csharp
private async Task ReadMyDataAsync(string key)
{
    // recover from local settings
    var dataFromLocal = await ApplicationData.Current.LocalSettings.ReadAsync<MyData>(key);

    // recover from roaming settings
    var dataFromRoaming = await ApplicationData.Current.RoamingSettings.ReadAsync<MyData>(key);

    // process data ...
}
```

### Storage folder

- Save content to StorageFolder: saves the serialized MyData object in json to a file in the StorageFolder.

```csharp
private async Task SaveMyDataAsync(string key, MyData data)
{
    //save in local folder
    await ApplicationData.Current.LocalFolder.SaveAsync(key, data);

    //save in roaming folder
    if (ApplicationData.Current.IsRoamingStorageAvailable())
    {
        await ApplicationData.Current.RoamingFolder.SaveAsync(key, data);
    }
}
```

- Get content from StorageFolder:

```csharp
private async Task ReadMyDataAsync(string key)
{
    // recover from local folder
    var dataFromLocal = await ApplicationData.Current.LocalFolder.ReadAsync<MyData>(key);

    // recover from roaming folder
    var dataFromRoaming = await ApplicationData.Current.LocalFolder.ReadAsync<MyData>(key);

    // process data ...
}
```

- Save file to StorageFolder: Get the array of bytes of a file and save in the StorageFolder.

```csharp
private async Task SaveMyFileAsync(string fileName)
{
    var sampleFile = await KnownFolders.DocumentsLibrary.GetFileAsync(fileName);

    // get bytes from file
    byte[] fileContent;
    using (var stream = await sampleFile.OpenStreamForReadAsync())
    {
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            fileContent = memoryStream.ToArray();
        }
    }

    //save in local folder
    await ApplicationData.Current.LocalFolder.SaveFileAsync(fileContent, fileName);

    //save in roaming folder
    if (ApplicationData.Current.IsRoamingStorageAvailable())
    {
        await ApplicationData.Current.RoamingFolder.SaveSaveFileAsyncAsync(fileContent, fileName);
    }
}
```

- Get file from StorageFolder.

```csharp
private async Task ReadMyFileAsync(string fileName)
{
    // recover from local folder
    var dataFromLocal = await ApplicationData.Current.LocalFolder.ReadFileAsync(fileName);

    // recover from roaming folder
    var dataFromRoaming = await ApplicationData.Current.LocalFolder.ReadFileAsync(fileName);

    //process files ...
}
```
