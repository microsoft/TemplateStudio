Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.Storage.Streams
Imports Param_ItemNamespace.Models

Namespace Helpers
    ' TODO WTS: Share Source Feature
    '  This extension, in conjuntion with the ShareSourceData class and ShareSourceItem, eases the development
    '  and helps to maximize the target applications for your shared data.
    '
    '  Please, refer to https://docs.microsoft.com/windows/uwp/app-to-app/share-data for further details.
    '
    ' To start sharing data from your pages / views you will need to follow these steps:
    '
    ' - Step 1. Setup a DataTransferManager object in your page / view and add a DataRequested event handler
    '   (i.e. OnDataRequested) to be called whenever the user invokes share.
    '
    ' - Step 2. Within the OnDataRequested event handler create a ShareSourceData instance and add the data you want to share.
    '
    ' - Step 3. Call the SetData extension method before leaving the event handler (i.e. args.Request.SetData(shareSourceData))
    '
    ' - Step 4. Call the DataTransferManager.ShowShareUI method from your command or handler to start the sharing action
    '
    ' Other considerations:
    '  - You can also register to the DataPackage ShareComplete event handler (args.Request.Data.ShareCompleted) to run code when the sharing operation ends.
    '    Be sure to unregister the ShareComplete event handler when done.
    Module DataRequestExtensions
        <Extension>
        Sub SetData(dataRequest As DataRequest, config As ShareSourceFeatureData)
            Dim deferral = dataRequest.GetDeferral()
            Try
                Dim requestData = dataRequest.Data
                requestData.Properties.Title = config.Title
                If Not String.IsNullOrEmpty(config.Description) Then
                    requestData.Properties.Description = config.Description
                End If

                Dim storageItems = New List(Of IStorageItem)()
                For Each dataItem In config.Items
                    Select Case dataItem.DataType
                        Case ShareSourceFeatureItemType.Text
                            requestData.SetText(dataItem.Text)
                        Case ShareSourceFeatureItemType.WebLink
                            requestData.SetWebLink(dataItem.WebLink)
                        Case ShareSourceFeatureItemType.ApplicationLink
                            requestData.SetApplicationLink(dataItem.ApplicationLink)
                        Case ShareSourceFeatureItemType.Html
                            Dim htmlFormat = HtmlFormatHelper.CreateHtmlFormat(dataItem.Html)
                            requestData.SetHtmlFormat(htmlFormat)
                        Case ShareSourceFeatureItemType.Image
                            requestData.FillImage(dataItem.Image, storageItems)
                        Case ShareSourceFeatureItemType.StorageItems
                            requestData.FillStorageItems(dataItem.StorageItems, storageItems)
                        Case ShareSourceFeatureItemType.DeferredContent
                            requestData.FillDeferredContent(dataItem.DeferredDataFormatId, dataItem.GetDeferredDataAsyncFunc)
                    End Select
                Next

                If storageItems.Any() Then
                    requestData.SetStorageItems(storageItems)
                End If
            Finally
                deferral.Complete()
            End Try
        End Sub

        <Extension>
        Private Sub FillImage(requestData As DataPackage, image As StorageFile, storageItems As List(Of IStorageItem))
            storageItems.Add(image)
            Dim streamReference = RandomAccessStreamReference.CreateFromFile(image)
            requestData.Properties.Thumbnail = streamReference
            requestData.SetBitmap(streamReference)
        End Sub

        <Extension>
        Private Sub FillStorageItems(requestData As DataPackage, sourceItems As IEnumerable(Of IStorageItem), storageItems As List(Of IStorageItem))
            For Each item In sourceItems
                storageItems.Add(item)
            Next
        End Sub

        <Extension>
        Private Sub FillDeferredContent(requestData As DataPackage, deferredDataFormatId As String, getDeferredDataAsyncFunc As Func(Of Task(Of Object)))
            requestData.SetDataProvider(deferredDataFormatId, Async Sub(providerRequest)
                Dim deferral = providerRequest.GetDeferral()
                Try
                    Dim deferredData = Await getDeferredDataAsyncFunc()
                    providerRequest.SetData(deferredData)
                Finally
                    deferral.Complete()
                End Try
            End Sub)
        End Sub
    End Module
End Namespace
