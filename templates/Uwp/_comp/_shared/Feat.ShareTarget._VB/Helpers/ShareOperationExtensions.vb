Imports Windows.ApplicationModel.DataTransfer
Imports Windows.ApplicationModel.DataTransfer.ShareTarget
Imports Windows.Storage.Streams
Imports Windows.Storage

Namespace Helpers
    Public Module ShareOperationExtensions

        <Extension>
        Public Async Function GetApplicationLinkAsync(shareOperation As ShareOperation) As Task(Of Uri)
            Return Await GetOperationDataAsync(Of Uri)(shareOperation, StandardDataFormats.ApplicationLink)
        End Function

        <Extension>
        Public Async Function GetBitmapAsync(shareOperation As ShareOperation) As Task(Of RandomAccessStreamReference)
            Return Await GetOperationDataAsync(Of RandomAccessStreamReference)(shareOperation, StandardDataFormats.Bitmap)
        End Function

        <Extension>
        Public Async Function GetHtmlFormatAsync(shareOperation As ShareOperation) As Task(Of String)
            Return Await GetOperationDataAsync(Of String)(shareOperation, StandardDataFormats.Html)
        End Function

        <Extension>
        Public Async Function GetRtfAsync(shareOperation As ShareOperation) As Task(Of String)
            Return Await GetOperationDataAsync(Of String)(shareOperation, StandardDataFormats.Rtf)
        End Function

        <Extension>
        Public Async Function GetStorageItemsAsync(shareOperation As ShareOperation) As Task(Of IReadOnlyList(Of IStorageItem))
            Return Await GetOperationDataAsync(Of IReadOnlyList(Of IStorageItem))(shareOperation, StandardDataFormats.StorageItems)
        End Function

        <Extension>
        Public Async Function GetTextAsync(shareOperation As ShareOperation) As Task(Of String)
            Return Await GetOperationDataAsync(Of String)(shareOperation, StandardDataFormats.Text)
        End Function

        <Extension>
        Public Async Function GetWebLinkAsync(shareOperation As ShareOperation) As Task(Of Uri)
            Return TryCast(Await GetOperationDataAsync(Of Uri)(shareOperation, StandardDataFormats.WebLink), Uri)
        End Function

        Private Async Function GetOperationDataAsync(Of T As Class)(shareOperation As ShareOperation, dataFormat As String) As Task(Of T)
            Try
                If dataFormat = StandardDataFormats.ApplicationLink Then
                    Return TryCast(Await shareOperation.Data.GetApplicationLinkAsync(), T)
                End If

                If dataFormat = StandardDataFormats.Bitmap Then
                    Return TryCast(Await shareOperation.Data.GetBitmapAsync(), T)
                End If

                If dataFormat = StandardDataFormats.Html Then
                    Return TryCast(Await shareOperation.Data.GetHtmlFormatAsync(), T)
                End If

                If dataFormat = StandardDataFormats.Rtf Then
                    Return TryCast(Await shareOperation.Data.GetRtfAsync(), T)
                End If

                If dataFormat = StandardDataFormats.StorageItems Then
                    Return TryCast(Await shareOperation.Data.GetStorageItemsAsync(), T)
                End If

                If dataFormat = StandardDataFormats.Text Then
                    Return TryCast(Await shareOperation.Data.GetTextAsync(), T)
                End If

                If dataFormat = StandardDataFormats.WebLink Then
                    Return TryCast(Await shareOperation.Data.GetWebLinkAsync(), T)
                End If
            Catch generatedExceptionName As Exception
                Return Nothing
            End Try

            Return Nothing
        End Function
    End Module
End Namespace
