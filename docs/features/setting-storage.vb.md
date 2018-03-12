# Setting Storage Extensions

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
```vbnet
Imports MyApp.Helpers
```

We will also use a example class as a content model:
```vbnet
Public Class MyData
    Public Property Id As Integer
    Public Property Name As String
End Class
```
### Application Data

- Save to ApplicationData: saves MyData object as a key-value pair where the value is serialized in json.

```vbnet
Private Async Function SaveMyDataAsync() As Task
    If ApplicationData.Current.IsRoamingStorageAvailable() Then
        Dim key = "MyDataKey"
        Dim myData = New MyData With {.Id = 1, .Name = "My Name"}
        Await ApplicationData.Current.LocalSettings.SaveAsync(key, myData)
    End If
End Function
```

- Get content from ApplicationData:
```vbnet
Private Async Function ReadMyDataAsync() As Task(Of MyData)
    Dim key = "MyDataKey"
    Return Await ApplicationData.Current.LocalSettings.ReadAsync(Of MyData)(key)
End Function
```
### Storage folder
- Save content to StorageFolder: saves the serialized MyData object in json to a file in the StorageFolder.
```vbnet
Private Async Function SaveMyDataAsync() As Task
    If ApplicationData.Current.IsRoamingStorageAvailable() Then
        Dim key = "MyDataKey"
        Dim myData = New MyData With {.Id = 1, .Name = "My Name"}
        Await ApplicationData.Current.LocalFolder.SaveAsync(key, myData)
    End If
End Function
```

- Get content from StorageFolder:
```vbnet
Private Async Function ReadMyDataAsync() As Task(Of MyData)
    Dim key = "MyDataKey"
    Return Await ApplicationData.Current.LocalFolder.ReadAsync(Of MyData)(key)
End Function
```

- Save file to StorageFolder: Save file in the StorageFolder.
```vbnet
Private Async Function SaveMyFileAsync() As Task
    Dim fileName = "myFileName"
    Dim sampleFile = Await KnownFolders.DocumentsLibrary.GetFileAsync(fileName)
    Dim fileContent As Byte()
    Using stream = Await sampleFile.OpenStreamForReadAsync()
        Using memoryStream = New MemoryStream()
            stream.CopyTo(memoryStream)
            fileContent = memoryStream.ToArray()
        End Using
    End Using

    Await ApplicationData.Current.LocalFolder.SaveFileAsync(fileContent, fileName)
End Function
```

- Get file from StorageFolder.
```vbnet
Private Async Function ReadMyFileAsync() As Task
    Dim fileName = "myFileName"
    Dim myFile = ApplicationData.Current.LocalFolder.ReadFileAsync(fileName)
    'process file ...
End Function
```
