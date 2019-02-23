Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace ViewModels
    Public Class ContentGridViewDetailViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _item As SampleOrder

        Public Property Item As SampleOrder
            Get
                Return _item
            End Get
            Set(value As SampleOrder)
                [Set](_item, value)
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub Initialize(orderId As Long)
            Dim data = SampleDataService.GetContentGridData()
            Item = data.First(Function(i) i.OrderId = orderId)
        End Sub

    End Class
End Namespace
