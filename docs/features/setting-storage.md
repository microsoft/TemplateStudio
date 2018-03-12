# Setting Storage Extensions

:heavy_exclamation_mark: There is also a version of [this document with code samples in VB.Net](./setting-storage.vb.md) :heavy_exclamation_mark: |
----------------------------------------------------------------------------------------------------------------------------------------------- |

SettingsStorageExtensions is a helper class that groups extension methods to facilitate the use of serialization, handling and storing of any content in a UWP application. This extension wraps the handling and storing of content (for example: custom content, key-values, files, etc.) in the same way as simple settings for a consistent interface.

## Advantages:
 - All methods of handling and storing content can be found in the same class and using a similar nomenclature. 
 - Facilitate the serialization and deserialization of data content and make it transparent for the developers.
 - Using methods that avoid problems in the serialization process (for example: big files that can take a long time to process).
 - All methods automatically give default values back if the key you're trying to read doesn't exist.
 - It makes working with byte arrays as easy as working with non-binary data (which can sometimes be complicated.)

This helper contains extension methods for handling and storing content in:
 - StorageFolder
 - ApplicationData

More info: [https://docs.microsoft.com/es-es/windows/uwp/design/app-settings/store-and-retrieve-app-data](https://docs.microsoft.com/es-es/windows/uwp/design/app-settings/store-and-retrieve-app-data).


## Examples of use:

To use these extension methods you have to add a reference to:
```csharp
using MyApp.Helpers;
```

We will also use a example class as a content model:
```csharp
public class MyData
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```
### Application Data

- Save to ApplicationData: saves MyData object as a key-value pair where the value is serialized in json.

```csharp
private async Task SaveMyDataAsync()
{
    if (ApplicationData.Current.IsRoamingStorageAvailable())
    {
        var key = "MyDataKey";
        var myData = new MyData { Id = 1, Name = "My Name" };

        await ApplicationData.Current.LocalSettings.SaveAsync(key, myData);
    }
}
```

- Get content from ApplicationData:
```csharp
private async Task<MyData> ReadMyDataAsync()
{
    var key = "MyDataKey";
    return await ApplicationData.Current.LocalSettings.ReadAsync<MyData>(key);
}
```
### Storage folder
- Save content to StorageFolder: saves the serialized MyData object in json to a file in the StorageFolder.
```csharp
private async Task SaveMyDataAsync()
{
    if (ApplicationData.Current.IsRoamingStorageAvailable())
    {
        var key = "MyDataKey";
        var myData = new MyData { Id = 1, Name = "My Name" };

        await ApplicationData.Current.LocalFolder.SaveAsync(key, myData);
    }
}
```

- Get content from StorageFolder:
```csharp
private async Task<MyData> ReadMyDataAsync()
{
    var key = "MyDataKey";
    return await ApplicationData.Current.LocalFolder.ReadAsync<MyData>(key);
}
```

- Save file to StorageFolder: Save file in the StorageFolder.
```csharp
private async Task SaveMyFileAsync()
{
    var fileName = "myFileName";
    var sampleFile = await KnownFolders.DocumentsLibrary.GetFileAsync(fileName);

    byte[] fileContent;
    using (var stream = await sampleFile.OpenStreamForReadAsync())
    {
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            fileContent = memoryStream.ToArray();
        }
    }

    await ApplicationData.Current.LocalFolder.SaveFileAsync(fileContent, fileName);            
}
```

- Get file from StorageFolder.
```csharp
private async Task ReadMyFileAsync()
{
    var fileName = "myFileName";
    var myFile = await ApplicationData.Current.LocalFolder.ReadFileAsync(fileName);

    //process file ...
}
```
