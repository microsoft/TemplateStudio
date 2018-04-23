Imports Windows.ApplicationModel.DataTransfer

Namespace Models

    Public Class DragDropCompletedData

        Public Property DropResult As DataPackageOperation

        Public Property Items As IReadOnlyList(Of Object)
    End Class
End Namespace
