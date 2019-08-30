# Setting Storage Extensions

Settings Storage provides extension methods that make working with application data simpler. It handles loading, saving, and serializing both files and individual pieces of data in any of the application folders.

[Find out more about storage and the ApplicationData class.](https://docs.microsoft.com/uwp/api/windows.storage.applicationdata)

---

:heavy_exclamation_mark: There is also a version of [this document with code samples in C#](./setting-storage.md) :heavy_exclamation_mark: |
---------------------------------------------------------------------------------------------------------------------------------------- |

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

```vb
Imports {YourAppName}.Helpers
```

We will also use a example class as a content model:

```vb
Public Class MyData
    Public Property Id As Integer
    Public Property Name As String
End Class
```

### Application Data Container

- Save to ApplicationDataContainer: saves MyData object as a key-value pair where the value is serialized in json.

```vb
Private Async Function SaveMyDataAsync(ByVal key As String, ByVal data As MyData) As Task

    'save in local settings
    ApplicationData.Current.LocalSettings.SaveAsync(key, data)

    'save in roaming settings
    If ApplicationData.Current.IsRoamingStorageAvailable Then
        ApplicationData.Current.RoamingSettings.SaveAsync(key, data)
    End If

End Function
```

- Get content from ApplicationData

```vb
Private Async Function ReadMyDataAsync(ByVal key As String) As Task

    ' recover from local settings
    Dim dataFromLocal = ApplicationData.Current.LocalSettings.ReadAsync(Of MyData)(key)

    ' recover from roaming settings
    Dim dataFromRoaming = ApplicationData.Current.RoamingSettings.ReadAsync(Of MyData)(key)

    ' process data ...

End Function
```

### Storage folder

- Save content to StorageFolder: saves the serialized MyData object in json to a file in the StorageFolder.

```vb
Private Async Function SaveMyDataAsync(ByVal key As String, ByVal data As MyData) As Task

    'save in local folder
    ApplicationData.Current.LocalFolder.SaveAsync(key, data)

    'save in roaming folder
    If ApplicationData.Current.IsRoamingStorageAvailable Then
        ApplicationData.Current.RoamingFolder.SaveAsync(key, data)
    End If

End Function
```

- Get content from StorageFolder

```vb
Private Async Function ReadMyDataAsync(ByVal key As String) As Task

    ' recover from local folder
    Dim dataFromLocal = ApplicationData.Current.LocalFolder.ReadAsync(Of MyData)(key)

    ' recover from roaming folder
    Dim dataFromRoaming = ApplicationData.Current.LocalFolder.ReadAsync(Of MyData)(key)

    ' process data ...

End Function
```

- Save file to StorageFolder: Get the array of bytes of a file and save in the StorageFolder.

```vb
Private Async Function SaveMyFileAsync(ByVal fileName As String) As Task

    Dim sampleFile = KnownFolders.DocumentsLibrary.GetFileAsync(fileName)

    ' get bytes from file
    Dim fileContent As Byte()
    Using stream = Await sampleFile.OpenStreamForReadAsync()
        Using memoryStream = New MemoryStream()
            stream.CopyTo(memoryStream)
            fileContent = memoryStream.ToArray()
        End Using
    End Using

    'save in local folder
    ApplicationData.Current.LocalFolder.SaveFileAsync(fileContent, fileName)

    'save in roaming folder
    If ApplicationData.Current.IsRoamingStorageAvailable Then
        ApplicationData.Current.RoamingFolder.SaveSaveFileAsyncAsync(fileContent, fileName)
    End If

End Function
```

- Get file from StorageFolder.

```vb
Private Async Function ReadMyFileAsync(ByVal fileName As String) As Task

    ' recover from local folder
    Dim dataFromLocal = ApplicationData.Current.LocalFolder.ReadFileAsync(fileName)

    ' recover from roaming folder
    Dim dataFromRoaming = ApplicationData.Current.LocalFolder.ReadFileAsync(fileName)

    'process files ...

End Function
```
