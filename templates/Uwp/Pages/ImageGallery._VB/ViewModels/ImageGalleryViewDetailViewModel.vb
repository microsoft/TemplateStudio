Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace ViewModels
    Public Class ImageGalleryViewDetailViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private Shared _image As UIElement

        Private _selectedImage As Object

        Public Property SelectedImage As Object
            Get
                Return _selectedImage
            End Get
            Set
                [Param_Setter](_selectedImage, value)
                ImagesNavigationHelper.UpdateImageId(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey, DirectCast(SelectedImage, SampleImage).ID)
            End Set
        End Property

        Public Property Source As ObservableCollection(Of SampleImage) = New ObservableCollection(Of SampleImage)

        Public Sub New()
        End Sub

        Public Async Function LoadDataAsync() As Task
            Source.Clear()

            ' TODO WTS: Replace this with your actual data
            Dim data = Await SampleDataService.GetImageGalleryDataAsync("ms-appx:///Assets")
            For Each item As SampleImage In data
                Source.Add(item)
            Next
        End Function

        Public Sub Initialize(selectedImageId As String, navigationMode as NavigationMode)
            If Not String.IsNullOrEmpty(selectedImageId) AndAlso navigationMode = NavigationMode.New Then
                SelectedImage = Source.FirstOrDefault(Function(i) i.ID = selectedImageId)
            Else
                selectedImageId = ImagesNavigationHelper.GetImageId(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey)
                If Not String.IsNullOrEmpty(selectedImageId) Then
                    SelectedImage = Source.FirstOrDefault(Function(i) i.ID = selectedImageId)
                End If
            End If
        End Sub
    End Class
End Namespace
