Imports Windows.ApplicationModel.DataTransfer
Imports Windows.ApplicationModel.DataTransfer.ShareTarget
Imports Windows.Storage
Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Models

Namespace Views
    ' TODO WTS: This page exists purely as an example of how to launch a specific page
    ' in response to a protocol launch and pass it a value. It is expected that you will
    ' delete this page once you have changed the handling of a protocol launch to meet your
    ' needs and redirected to another of your pages.
    Public NotInheritable Partial Class ShareTargetFeaturePage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _shareOperation As ShareOperation

        Private _sharedData As SharedDataModelBase

        Public Property SharedData As SharedDataModelBase
            Get
                Return _sharedData
            End Get
            Set
                [Set](_sharedData, value)
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            ' TODO WTS: Configure your Share Target Declaration to allow other data formats.
            ' Share Target declarations are defined in Package.appxmanifest.
            ' Current declarations allow to share WebLink and image files with the app.
            ' ShareTarget can be tested sharing the WebLink from Microsoft Edge or sharing images from Windows Photos.

            ' ShareOperation contains all the information required to handle the action.
            MyBase.OnNavigatedTo(e)

            ' TODO WTS: Customize SharedDataModelBase or derived classes adding properties for data that you need to extract from _shareOperation
            _shareOperation = TryCast(e.Parameter, ShareOperation)

            If _shareOperation.Data.Contains(StandardDataFormats.WebLink) Then
                Dim newSharedData = New SharedDataWebLinkModel() With {
                    .Title = _shareOperation.Data.Properties.Title,
                    .PageTitle = "ShareTargetFeature_WebLinkTitle".GetLocalized(),
                    .DataFormat = StandardDataFormats.WebLink
                }
                newSharedData.Uri = Await _shareOperation.GetWebLinkAsync()
                SharedData = newSharedData
            End If

            If _shareOperation.Data.Contains(StandardDataFormats.StorageItems) Then
                Dim newSharedData = New SharedDataStorageItemsModel() With {
                    .Title = _shareOperation.Data.Properties.Title,
                    .PageTitle = "ShareTargetFeature_ImagesTitle".GetLocalized(),
                    .DataFormat = StandardDataFormats.StorageItems
                }
                Dim files = Await _shareOperation.GetStorageItemsAsync()
                For Each file As IStorageFile In files
                    Dim storageFile = TryCast(file, StorageFile)
                    If storageFile IsNot Nothing Then
                        Using inputStream = Await storageFile.OpenReadAsync()
                            Dim img = New BitmapImage()
                            img.SetSource(inputStream)
                            newSharedData.Images.Add(img)
                        End Using
                    End If
                Next

                SharedData = newSharedData
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
                Return
            End If

            storage = value
            OnPropertyChanged(propertyName)
        End Sub

        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Private Sub CompleteButton_Click(sender As Object, e As RoutedEventArgs)
            ' TODO WTS: Implement the actions you want to realize with the shared data before completing the share operation.
            ' More details at https://docs.microsoft.com/en-us/windows/uwp/app-to-app/receive-data
            _shareOperation.ReportCompleted()
        End Sub
    End Class
End Namespace
