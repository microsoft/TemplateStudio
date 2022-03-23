Imports Windows.Storage
Imports Param_RootNamespace.Helpers

Namespace Models
    Public Class ShareSourceFeatureData

        Public Property Title As String

        Public Property Description As String

        Friend Property Items As List(Of ShareSourceFeatureItem)

        Public Sub New(title As String, Optional desciption As String = Nothing)
            If String.IsNullOrEmpty(title) Then
                Throw New ArgumentException("The parameter title can not be null or empty.", NameOf(title))
            End If

            Items = New List(Of ShareSourceFeatureItem)()
            Title = title
            Description = desciption
        End Sub

        Public Sub SetText(text As String)
            If String.IsNullOrEmpty(text) Then
                Throw New ArgumentException("The parameter title can not be null or empty.", nameof(text))
            End If

            Items.Add(ShareSourceFeatureItem.FromText(text))
        End Sub

        Public Sub SetWebLink(webLink As Uri)
            If webLink Is Nothing Then
                Throw New ArgumentNullException(NameOf(webLink))
            End If

            Items.Add(ShareSourceFeatureItem.FromWebLink(webLink))
        End Sub

        ' To share a link to your app you must first register it to handle URI activation
        ' More details at https://docs.microsoft.com/windows/uwp/launch-resume/handle-uri-activation
        Public Sub SetApplicationLink(applicationLink As Uri)
            If applicationLink Is Nothing Then
                Throw New ArgumentNullException(NameOf(applicationLink))
            End If

            Items.Add(ShareSourceFeatureItem.FromApplicationLink(applicationLink))
        End Sub

        Public Sub SetHtml(html As String)
            If String.IsNullOrEmpty(html) Then
                Throw New ArgumentException("The Parameter html is null or empty.", nameof(html))
            End If

            Items.Add(ShareSourceFeatureItem.FromHtml(html))
        End Sub

        Public Sub SetImage(image As StorageFile)
            If image Is Nothing Then
                Throw New ArgumentNullException(NameOf(image))
            End If

            Items.Add(ShareSourceFeatureItem.FromImage(image))
        End Sub

        Public Sub SetStorageItems(storageItems As IEnumerable(Of IStorageItem))
            If storageItems Is Nothing OrElse Not storageItems.Any() Then
                Throw New ArgumentException("The Parameter StorageItems is null or does not contains any element.", nameof(storageItems))
            End If

            Items.Add(ShareSourceFeatureItem.FromStorageItems(storageItems))
        End Sub

        ' Use this method to add content to share when you do not want to process the data until the target app actually requests it.
        ' The deferredDataFormatId parameter must be a const value from StandardDataFormats class.
        ' The getDeferredDataAsyncFunc parameter is the function that returns the object you want to share.
        Public Sub SetDeferredContent(deferredDataFormatId As String, getDeferredDataAsyncFunc As Func(Of Task(Of Object)))
            If String.IsNullOrEmpty(deferredDataFormatId) Then
                Throw New ArgumentException("The Parameter DeferredDataFormatId is null or does not contains any element.", nameof(deferredDataFormatId))
            End If

            Items.Add(ShareSourceFeatureItem.FromDeferredContent(deferredDataFormatId, getDeferredDataAsyncFunc))
        End Sub
    End Class
End Namespace
