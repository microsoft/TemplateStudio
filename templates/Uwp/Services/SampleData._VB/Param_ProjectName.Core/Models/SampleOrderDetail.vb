Namespace Models
    ' TODO WTS: Remove this class once your pages/features are using your data.
    ' This is used by the SampleDataService.
    ' It is the model class we use to display data on pages like Grid, Chart, and Master Detail.
    Public Class SampleOrderDetail
        Public Property ProductID As Long

        Public Property ProductName As String

        Public Property Quantity As Integer

        Public Property Discount As Double

        Public Property QuantityPerUnit As String

        Public Property UnitPrice As Double

        Public Property CategoryName As String

        Public Property CategoryDescription As String

        Public Property Total As Double

        Public ReadOnly Property ShortDescription As String
            Get
                Return $"Product ID: {ProductID} - {ProductName}"
            End Get
        End Property
    End Class
End Namespace