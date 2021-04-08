Namespace Models
    ' TODO WTS: Remove this class once your pages/features are using your data.
    ' This is used by the SampleDataService.
    ' It is the model class we use to display data on pages like Grid, Chart, and List Detail.
    Public Class SampleOrder
        Public Property OrderID As Long

        Public Property OrderDate As DateTime

        Public Property RequiredDate As DateTime

        Public Property ShippedDate As DateTime

        Public Property ShipperName As String

        Public Property ShipperPhone As String

        Public Property Freight As Double

        Public Property Company As String

        Public Property ShipTo As String

        Public Property OrderTotal As Double

        Public Property Status As String

        Public ReadOnly Property Symbol As Char
            Get
                Return Convert.ToChar(SymbolCode)
            End Get
        End Property

        Public Property SymbolCode As Integer

        Public Property Details As ICollection(Of SampleOrderDetail)

        Public Overrides Function ToString() As String
            Return $"{Company} {Status}"
        End Function

        Public ReadOnly Property ShortDescription As String
            Get
                Return $"Order ID: {OrderID}"
            End Get
        End Property
    End Class
End Namespace