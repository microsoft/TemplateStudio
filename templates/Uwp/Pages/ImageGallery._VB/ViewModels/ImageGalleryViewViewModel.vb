Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services

Namespace ViewModels
    Public Class ImageGalleryViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Public Const ImageGalleryViewSelectedIdKey As String = "ImageGalleryViewSelectedIdKey"

        Public Property Source As ObservableCollection(Of SampleImage) = New ObservableCollection(Of SampleImage)

        Public ReadOnly Property ItemSelectedCommand As ICommand = new RelayCommand(Of ItemClickEventArgs)(Sub(args) OnsItemSelected(args))

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
    End Class
End Namespace
