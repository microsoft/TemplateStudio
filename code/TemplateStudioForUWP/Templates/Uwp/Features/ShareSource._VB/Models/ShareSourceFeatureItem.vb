Imports Windows.Storage

Namespace Models

    Friend Enum ShareSourceFeatureItemType
        Text = 0
        WebLink = 1
        ApplicationLink = 2
        Html = 3
        Image = 4
        StorageItems = 5
        DeferredContent = 6
    End Enum

    Friend Class ShareSourceFeatureItem

        Public Property DataType As ShareSourceFeatureItemType

        Public Property Text As String

        Public Property WebLink As Uri

        Public Property ApplicationLink As Uri

        Public Property Html As String

        Public Property Image As StorageFile

        Public Property StorageItems As IEnumerable(Of IStorageItem)

        Public Property DeferredDataFormatId As String

        Public Property GetDeferredDataAsyncFunc As Func(Of Task(Of Object))

        Private Sub New(dataType As ShareSourceFeatureItemType)
            DataType = dataType
        End Sub

        Friend Shared Function FromText(text As String) As ShareSourceFeatureItem
            Return New ShareSourceFeatureItem(ShareSourceFeatureItemType.Text) With {.Text = text}
        End Function

        Friend Shared Function FromWebLink(webLink As Uri) As ShareSourceFeatureItem
            Return New ShareSourceFeatureItem(ShareSourceFeatureItemType.WebLink) With {.WebLink = webLink}
        End Function

        Friend Shared Function FromApplicationLink(applicationLink As Uri) As ShareSourceFeatureItem
            Return New ShareSourceFeatureItem(ShareSourceFeatureItemType.ApplicationLink) With {.ApplicationLink = applicationLink}
        End Function

        Friend Shared Function FromHtml(html As String) As ShareSourceFeatureItem
            Return New ShareSourceFeatureItem(ShareSourceFeatureItemType.Html) With {.Html = html}
        End Function

        Friend Shared Function FromImage(image As StorageFile) As ShareSourceFeatureItem
            Return New ShareSourceFeatureItem(ShareSourceFeatureItemType.Image) With {.Image = image}
        End Function

        Friend Shared Function FromStorageItems(storageItems As IEnumerable(Of IStorageItem)) As ShareSourceFeatureItem
            Return New ShareSourceFeatureItem(ShareSourceFeatureItemType.StorageItems) With {.StorageItems = storageItems}
        End Function

        Friend Shared Function FromDeferredContent(deferredDataFormatId As String, getDeferredDataAsyncFunc As Func(Of Task(Of Object))) As ShareSourceFeatureItem
            Return New ShareSourceFeatureItem(ShareSourceFeatureItemType.DeferredContent) With {.DeferredDataFormatId = deferredDataFormatId, .GetDeferredDataAsyncFunc = getDeferredDataAsyncFunc}
        End Function
    End Class
End Namespace
