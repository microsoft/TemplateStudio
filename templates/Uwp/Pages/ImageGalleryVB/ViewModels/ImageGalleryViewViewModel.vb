Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Core.Models
Imports Param_ItemNamespace.Core.Services
Imports Param_ItemNamespace.Services

Namespace ViewModels
    Public Class ImageGalleryViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Public Const ImageGalleryViewSelectedIdKey As String = "ImageGalleryViewSelectedIdKey"

        Private _source As ObservableCollection(Of SampleImage)

        Public Property Source As ObservableCollection(Of SampleImage)
            Get
                Return _source
            End Get
            Set
                [Param_Setter](_source, value)
            End Set
        End Property

        Public ReadOnly Property ItemSelectedCommand As ICommand = new RelayCommand(Of ItemClickEventArgs)(Sub(args) OnsItemSelected(args))

        Public Sub New()
            ' TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData()
        End Sub
    End Class
End Namespace
