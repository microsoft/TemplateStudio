Namespace Models
    ' TODO WTS: Remove this class once your pages/features are using your data.
    ' This is used by the SampleDataService.
    ' It is the model class we use to display data on pages like Grid, Chart, and List Detail.
    Public Class SampleCompany
        Public Property CompanyID As String

        Public Property CompanyName As String

        Public Property ContactName As String

        Public Property ContactTitle As String

        Public Property Address As String

        Public Property City As String

        Public Property PostalCode As String

        Public Property Country As String

        Public Property Phone As String

        Public Property Fax As String

        Public Property Orders As ICollection(Of SampleOrder)
    End Class
End Namespace