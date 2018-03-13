Imports Windows.Storage
Imports Windows.Storage.Streams

Namespace Helpers
    ' Use these extension methods to store and retrieve local and roaming app data
    ' More details regarding storing and retrieving app data at https://docs.microsoft.com/windows/uwp/app-settings/store-and-retrieve-app-data
    Friend Module SettingsStorageExtensions

        Private Const FileExtension As String = ".json"

        <Extension>
        Public Function IsRoamingStorageAvailable(appData As ApplicationData) As Boolean
            Return appData.RoamingStorageQuota = 0
        End Function

        <Extension>
        Public Async Function SaveAsync(Of T)(folder As StorageFolder, name As String, content As T) As Task
            Dim file = Await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting)
            Dim fileContent = Await Json.StringifyAsync(content)

            Await FileIO.WriteTextAsync(file, fileContent)
        End Function

        <Extension>
        Public Async Function ReadAsync(Of T)(folder As StorageFolder, name As String) As Task(Of T)
            If Not IO.File.Exists(Path.Combine(folder.Path, GetFileName(name))) Then
                Return Nothing
            End If

            Dim file = Await folder.GetFileAsync($"{name}.json")
            Dim fileContent = Await FileIO.ReadTextAsync(file)

            Return Await Json.ToObjectAsync(Of T)(fileContent)
        End Function

        <Extension>
        Public Async Function SaveAsync(Of T)(settings As ApplicationDataContainer, key As String, value As T) As Task
            settings.SaveString(key, Await Json.StringifyAsync(value))
        End Function

        <Extension>
        Public Sub SaveString(settings As ApplicationDataContainer, key As String, value As String)
            settings.Values(key) = value
        End Sub

        <Extension>
        Public Async Function ReadAsync(Of T)(settings As ApplicationDataContainer, key As String) As Task(Of T)
            Dim obj As Object = Nothing

            If settings.Values.TryGetValue(key, obj) Then
                Return Await Json.ToObjectAsync(Of T)(DirectCast(obj, String))
            End If

            Return Nothing
        End Function

        <Extension>
        Public Async Function SaveFileAsync(folder As StorageFolder, content As Byte(), fileName As String, Optional options As CreationCollisionOption = CreationCollisionOption.ReplaceExisting) As Task(Of StorageFile)
            If content Is Nothing Then
                Throw New ArgumentNullException(NameOf(content))
            End If

            If String.IsNullOrEmpty(fileName) Then
                Throw New ArgumentException("ExceptionSettingsStorageExtensionsFileNameIsNullOrEmpty".GetLocalized(), NameOf(fileName))
            End If

            Dim storageFile = Await folder.CreateFileAsync(fileName, options)
            Await FileIO.WriteBytesAsync(storageFile, content)
            Return storageFile
        End Function

        <Extension>
        Public Async Function ReadFileAsync(folder As StorageFolder, fileName As String) As Task(Of Byte())
            Dim item = Await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(False)

            If (item IsNot Nothing) AndAlso item.IsOfType(StorageItemTypes.File) Then
                Dim storageFile = Await folder.GetFileAsync(fileName)
                Dim content As Byte() = Await storageFile.ReadBytesAsync()
                Return content
            End If

            Return Nothing
        End Function

        <Extension>
        Public Async Function ReadBytesAsync(file As StorageFile) As Task(Of Byte())
            If file IsNot Nothing Then
                Using stream As IRandomAccessStream = Await file.OpenReadAsync()
                    Using reader = New DataReader(stream.GetInputStreamAt(0))
                        Await reader.LoadAsync(CUInt(stream.Size))
                        Dim bytes = New Byte(stream.Size - 1) {}
                        reader.ReadBytes(bytes)
                        Return bytes
                    End Using
                End Using
            End If

            Return Nothing
        End Function

        Private Function GetFileName(name As String) As String
            Return String.Concat(name, FileExtension)
        End Function
    End Module
End Namespace
