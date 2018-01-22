Namespace Models
    ' TODO WTS: Remove this class once your pages/features are using your data.
    ' This is used by the SampleDataService.
    ' It is the model class we use to display data on pages like Grid, Chart, and Master Detail.
    Public Class SampleOrder
        Public Property OrderId As Long

        Public Property OrderDate As DateTime

        Public Property Company As String

        Public Property ShipTo As String

        Public Property OrderTotal As Double

        Public Property Status As String

        Public Property Symbol As String

        Public ReadOnly Property SymbolAsChar As Char
            Get
                Return ChrW(Symbol)
            End Get
        End Property

        Public ReadOnly Property HashIdentIcon As String
            Get
                Return GetHashCode().ToString() & "-icon"
            End Get
        End Property

        Public ReadOnly Property HashIdentTitle As String
            Get
                Return GetHashCode().ToString() & "-title"
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return $"{Company} {Status}"
        End Function
    End Class
End Namespace
