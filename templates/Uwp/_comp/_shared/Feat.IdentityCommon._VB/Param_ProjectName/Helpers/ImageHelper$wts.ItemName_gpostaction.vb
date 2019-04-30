'{[{
Imports System.Runtime.InteropServices.WindowsRuntime
Imports Windows.Storage.Streams
'}]}
Namespace Helpers
    Module ImageHelper
'^^
'{[{
        Async Function ImageFromStringAsync(data As String) As Task(Of BitmapImage)
            Dim byteArray = Convert.FromBase64String(data)
            Dim image = New BitmapImage()

            Using stream = New InMemoryRandomAccessStream()
                Await stream.WriteAsync(byteArray.AsBuffer())
                stream.Seek(0)
                Await image.SetSourceAsync(stream)
            End Using

            Return image
        End Function

        Function ImageFromAssetsFile(fileName As String) As BitmapImage
            Dim image = New BitmapImage(New Uri($"ms-appx:///Assets/{fileName}"))
            Return image
        End Function
'}]}
    End Module
End Namespace
