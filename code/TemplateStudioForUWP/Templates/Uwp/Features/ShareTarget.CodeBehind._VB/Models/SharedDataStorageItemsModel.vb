Namespace Models
    Public Class SharedDataStorageItemsModel
        Inherits SharedDataModelBase

        Public ReadOnly Property Images As List(Of ImageSource) = new List(Of ImageSource)

        Public Sub New()
            MyBase.New()
        End Sub
    End Class
End Namespace
