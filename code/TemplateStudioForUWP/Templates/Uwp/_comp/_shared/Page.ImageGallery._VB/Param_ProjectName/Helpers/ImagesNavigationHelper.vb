Namespace Helpers
    Module ImagesNavigationHelper
        Private _imageGalleriesHistories As Dictionary(Of String, Stack(Of String)) = New Dictionary(Of String, Stack(Of String))()

        Sub AddImageId(imageGalleryId As String, imageId As String)
            Dim stack = GetStack(imageGalleryId)
            stack.Push(imageId)
        End Sub

        Sub UpdateImageId(imageGalleryId As String, imageId As String)
            Dim stack = GetStack(imageGalleryId)

            If stack.Any() Then
                stack.Pop()
            End If

            stack.Push(imageId)
        End Sub

        Function GetImageId(imageGalleryId As String) As String
            Dim stack = GetStack(imageGalleryId)
            Return If(stack.Any(), stack.Peek(), String.Empty)
        End Function

        Sub RemoveImageId(imageGalleryId As String)
            Dim stack = GetStack(imageGalleryId)

            If stack.Any() Then
                stack.Pop()
            End If
        End Sub

        Private Function GetStack(imageGalleryId As String) As Stack(Of String)
            If Not _imageGalleriesHistories.Keys.Contains(imageGalleryId) Then
                _imageGalleriesHistories.Add(imageGalleryId, New Stack(Of String)())
            End If

            Return _imageGalleriesHistories(imageGalleryId)
        End Function
    End Module
End Namespace
