Imports Windows.ApplicationModel.DataTransfer
Imports Windows.ApplicationModel.DataTransfer.ShareTarget
Imports Windows.Storage

Imports Param_RootNamespace.Helpers

Namespace ViewModels
    Public Class SharedDataStorageItemsViewModel
        Inherits SharedDataViewModelBase

        Public ReadOnly Property Images As ObservableCollection(Of ImageSource) = new ObservableCollection(Of ImageSource)

        Public Sub New()
        End Sub

        Public Overrides Async Function LoadDataAsync(shareOperation As ShareOperation) As Task
            Await MyBase.LoadDataAsync(shareOperation)

            PageTitle = "ShareTargetFeature_ImagesTitle".GetLocalized()
            DataFormat = StandardDataFormats.StorageItems
            Dim files = Await shareOperation.GetStorageItemsAsync()
            For Each file As IStorageFile In files
                Dim storageFile = TryCast(file, StorageFile)
                If storageFile IsNot Nothing Then
                    Using inputStream = Await storageFile.OpenReadAsync()
                        Dim img = New BitmapImage()
                        img.SetSource(inputStream)
                        Images.Add(img)
                    End Using
                End If
            Next
        End Function
    End Class
End Namespace
