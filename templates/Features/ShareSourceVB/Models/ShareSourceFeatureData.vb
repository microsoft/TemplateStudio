Imports Windows.Storage

Namespace Models
    Public Class ShareSourceFeatureData

        Public Property Title As String

        Public Property Description As String

        Friend Property Items As List(Of ShareSourceFeatureItem)

        Public Sub New(ByVal title As String, ByVal Optional desciption As String = Nothing)
            If String.IsNullOrEmpty(title) Then
                Throw New ArgumentException($"The parameter '{NameOf(title)}' can not be null or empty.")
            End If

            Items = New List(Of ShareSourceFeatureItem)()
            Title = title
            Description = desciption
        End Sub

        Public Sub SetText(ByVal text As String)
            If String.IsNullOrEmpty(text) Then
                Throw New ArgumentException($"The parameter '{NameOf(text)}' is null or empty")
            End If

            Items.Add(ShareSourceFeatureItem.FromText(text))
        End Sub

        Public Sub SetWebLink(ByVal webLink As Uri)
            If webLink Is Nothing Then
                Throw New ArgumentNullException(NameOf(webLink))
            End If

            Items.Add(ShareSourceFeatureItem.FromWebLink(webLink))
        End Sub

        Public Sub SetApplicationLink(ByVal applicationLink As Uri)
            If applicationLink Is Nothing Then
                Throw New ArgumentNullException(NameOf(applicationLink))
            End If

            Items.Add(ShareSourceFeatureItem.FromApplicationLink(applicationLink))
        End Sub

        Public Sub SetHtml(ByVal html As String)
            If String.IsNullOrEmpty(html) Then
                Throw New ArgumentException($"Parameter '{NameOf(html)}' to share is null or empty.")
            End If

            Items.Add(ShareSourceFeatureItem.FromHtml(html))
        End Sub

        Public Sub SetImage(ByVal image As StorageFile)
            If image Is Nothing Then
                Throw New ArgumentNullException($"{NameOf(image)}")
            End If

            Items.Add(ShareSourceFeatureItem.FromImage(image))
        End Sub

        Public Sub SetStorageItems(ByVal storageItems As IEnumerable(Of IStorageItem))
            If storageItems Is Nothing OrElse Not storageItems.Any() Then
                Throw New ArgumentException($"Paramter '{NameOf(storageItems)}' is null or does not contains any element.")
            End If

            Items.Add(ShareSourceFeatureItem.FromStorageItems(storageItems))
        End Sub

        Public Sub SetDeferredContent(ByVal deferredDataFormatId As String, ByVal getDeferredDataAsyncFunc As Func(Of Task(Of Object)))
            If String.IsNullOrEmpty(deferredDataFormatId) Then
                Throw New ArgumentException($"Parameter '{NameOf(deferredDataFormatId)}' to share is null or empty")
            End If

            Items.Add(ShareSourceFeatureItem.FromDeferredContent(deferredDataFormatId, getDeferredDataAsyncFunc))
        End Sub
    End Class
End Namespace
