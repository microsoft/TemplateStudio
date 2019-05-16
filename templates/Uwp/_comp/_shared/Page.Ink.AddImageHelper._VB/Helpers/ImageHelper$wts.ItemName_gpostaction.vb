'{[{
Imports Windows.Storage
Imports Windows.Storage.Pickers
'}]}
Namespace Helpers
    Module ImageHelper
'^^
'{[{

        Async Function LoadImageFileAsync() As Task(Of StorageFile)
            Dim openPicker = New FileOpenPicker With {
                .SuggestedStartLocation = PickerLocationId.PicturesLibrary
            }
            openPicker.FileTypeFilter.Add(".png")
            openPicker.FileTypeFilter.Add(".jpeg")
            openPicker.FileTypeFilter.Add(".jpg")
            openPicker.FileTypeFilter.Add(".bmp")
            Dim imageFile = Await openPicker.PickSingleFileAsync()
            Return imageFile
        End Function

        Async Function GetBitmapFromImageAsync(file As StorageFile) As Task(Of BitmapImage)
            If file Is Nothing Then
                Return Nothing
            End If

            Try
                Using fileStream = Await file.OpenAsync(FileAccessMode.Read)
                    Dim bitmapImage = New BitmapImage()
                    Await bitmapImage.SetSourceAsync(fileStream)
                    Return bitmapImage
                End Using

            Catch ex As Exception
                Return Nothing
            End Try
        End Function
'}]}
    End Module
End Namespace
